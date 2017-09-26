using RiverMobile.Messages;
using RiverMobile.Models;
using System;
using System.Threading.Tasks;
using System.IO;
using JsonApiSerializer;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RiverMobile.Services
{
    public class StampBatchService : IStampBatchService
    {
        readonly IMessageService messageService;
        readonly IStampUploadService backgroundRiverApiService;

        static readonly string stampBatchFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StampBatch.json");
        static readonly JsonApiSerializerSettings jsonSerializerSettings = new JsonApiSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };

        public StampBatchService(
            IMessageService messageService,
            IStampUploadService backgroundRiverApiService)
        {
            this.messageService = messageService;
            this.backgroundRiverApiService = backgroundRiverApiService;

            messageService.Subscribe(this, async (object messenger, RecordStampMessage message) =>
            {
                await BatchStampsAsync(message.Stamp);
            });
        }
        public async Task BatchStampsAsync(Stamp newStamp)
        {
            var stamps = ReadStampBatch();

            stamps.Add(newStamp);

            if (stamps.Count >= 3)
            {
                foreach (var stamp in stamps)
                {
                    await backgroundRiverApiService.PostRiverModelAsync(stamp);
                    stamps.Remove(stamp);
                }
            }

            WriteStampBatch(stamps);
        }

        void WriteStampBatch(IList<Stamp> stamps)
        {
            var jsonContent = JsonConvert.SerializeObject(stamps, jsonSerializerSettings);
            File.WriteAllText(stampBatchFile, jsonContent); 
        }

        IList<Stamp> ReadStampBatch()
        {
            var jsonContent = File.ReadAllText(stampBatchFile);
            return JsonConvert.DeserializeObject<Stamp[]>(jsonContent, jsonSerializerSettings);
        }
    }
}
