using RiverMobile.Messages;
using RiverMobile.Models;
using System;
using System.Threading.Tasks;
using System.IO;
using JsonApiSerializer;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace RiverMobile.Services
{
    public class StampFile
    {
        public Guid Id;
        public string FilePath;
    }

    public class StampBatchService : IStampBatchService
    {
        readonly IMessageService messageService;
        readonly IStampUploadService backgroundRiverApiService;

        static readonly string stampBatchFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StampBatch.json");
        static readonly JsonApiSerializerSettings jsonSerializerSettings = new JsonApiSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };
        readonly IRiverApiService riverApiService;

        public StampBatchService(
            IMessageService messageService,
            IRiverApiService riverApiService,
            IStampUploadService backgroundRiverApiService)
        {
            this.riverApiService = riverApiService;
            this.messageService = messageService;
            this.backgroundRiverApiService = backgroundRiverApiService;

            messageService.Subscribe(this, (object messenger, RecordStampMessage message) =>
            {
                BatchStamps(message.Stamp);
            });

            File.WriteAllText(stampBatchFile, string.Empty);
        }
        public void BatchStamps(Stamp newStamp)
        {
            var stampFiles = ReadStampBatch() ?? new List<StampFile>();

            stampFiles.Add(WriteStamp(newStamp));

            if (stampFiles.Count >= 1)
            {
                foreach (var stampFile in stampFiles)
                {
                    var stampJson = File.ReadAllText(stampFile.FilePath);
                    var stamp = JsonConvert.DeserializeObject<Stamp>(stampJson, jsonSerializerSettings);
                    riverApiService.PostRiverModelAsync(stamp);
                    //backgroundRiverApiService.UploadStamp(stamp.FilePath);
                }
                stampFiles.RemoveAll(s => true);
            }

            WriteStampBatch(stampFiles);
        }

        StampFile WriteStamp(Stamp stamp)
        {
            var jsonContent = JsonConvert.SerializeObject(stamp, jsonSerializerSettings);
            var id = Guid.NewGuid();
            var stampFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), id.ToString());
            File.WriteAllText(stampFilePath, jsonContent);
            Console.WriteLine("You're writing a stamp");
            return new StampFile()
            {
                Id = id,
                FilePath = stampFilePath
            };
        }

        void WriteStampBatch(IList<StampFile> stamps)
        {
            var jsonContent = JsonConvert.SerializeObject(stamps, jsonSerializerSettings);
            File.WriteAllText(stampBatchFile, jsonContent);
        }

        List<StampFile> ReadStampBatch()
        {
            var jsonContent = File.ReadAllText(stampBatchFile);
            var stampFiles = JsonConvert.DeserializeObject<StampFile[]>(jsonContent);
            return stampFiles?.ToList() ?? new List<StampFile>();
        }
    }
}
