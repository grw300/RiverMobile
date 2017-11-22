using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CoreLocation;
using RiverMobile.Services;
using RiverMobile.Models;
using RiverMobile.Helpers;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;
using RiverMobile.Messages;

namespace RiverMobile.iOS.Services
{
    public class NearestNeighbors : INearestNeighbors
    {
        readonly IMessageService messageService;

        public HashSet<BeaconRegion> BeaconRegions { get; private set; } = new HashSet<BeaconRegion>();
        public Dictionary<BeaconRegion, HashSet<BeaconRegion>> Neighbors { get; private set; } = new Dictionary<BeaconRegion, HashSet<BeaconRegion>>();

        public NearestNeighbors(
            IMessageService messageService)
        {
            this.messageService = messageService;

            var GregBeet = new BeaconRegion("1F0FCF35-502E-2A5E-4BCA-BBDEE139A162", "com.GregWill.RiverBeetroot", 4500);
            var GregLemon = new BeaconRegion("49881822-DF49-C146-F648-7C30618C1CF8", "com.GregWill.RiverLemon", 36288);
            var GregCandy = new BeaconRegion("C70C3311-A575-290A-A310-F86AACED2A25", "com.GregWill.RiverCandy", 36646);
            var BenBeet = new BeaconRegion("B9407F30-F5F8-466E-AFF9-25556B57FE6D", "com.BenCelis.RiverBeetroot", 3505);
            var BenLemon = new BeaconRegion("702666BD-2369-48E9-AAE4-1022C95B0E8F", "com.BenCelis.RiverLemon", 19053);
            var BenCandy = new BeaconRegion("3887E468-4EF1-4D31-8B5B-B2A900326D1B", "com.BenCelis.RiverCandy", 19350);


            BeaconRegions.Add(GregBeet);
            BeaconRegions.Add(GregLemon);
            BeaconRegions.Add(GregCandy);
            BeaconRegions.Add(BenBeet);
            BeaconRegions.Add(BenLemon);
            BeaconRegions.Add(BenCandy);

            Neighbors.Add(BenBeet, new HashSet<BeaconRegion> { GregBeet, GregLemon, BenCandy });
            Neighbors.Add(BenCandy, new HashSet<BeaconRegion> { BenBeet, BenLemon });
            Neighbors.Add(BenLemon, new HashSet<BeaconRegion> { BenCandy, GregCandy });
            Neighbors.Add(GregBeet, new HashSet<BeaconRegion> { GregLemon, BenBeet, BenCandy });
            Neighbors.Add(GregCandy, new HashSet<BeaconRegion> { GregLemon, BenLemon });
            Neighbors.Add(GregLemon, new HashSet<BeaconRegion> { GregBeet, BenBeet, GregCandy });
        }

        public void RecordStamp(int location)
        {
            Settings.CurrentLocation = location;

            var stamp = new Stamp
            {
                Time = DateTime.UtcNow,
                Location = location,
                Personal = new Relationship<Personal>
                {
                    Data = JsonConvert.DeserializeObject<Personal>(Settings.UserJson, new JsonSerializerSettings())
                }
            };

            messageService.Send(new RecordStampMessage(stamp));
        }

    }
}