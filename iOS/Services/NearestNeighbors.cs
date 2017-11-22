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
    public class NearestNeighbors : CLLocationManagerDelegate, INearestNeighbors
    {
        IMessageService messageService;

        public HashSet<BeaconRegion> BeaconRegions { get; private set; } = new HashSet<BeaconRegion>();

        public NearestNeighbors(
            IMessageService messageService)
        {
            this.messageService = messageService;

            BeaconRegions.Add(new BeaconRegion("1F0FCF35-502E-2A5E-4BCA-BBDEE139A162", "com.GregWill.RiverBeetroot", 9228));
            BeaconRegions.Add(new BeaconRegion("49881822-DF49-C146-F648-7C30618C1CF8", "com.GregWill.RiverLemon", 15983));
            BeaconRegions.Add(new BeaconRegion("C70C3311-A575-290A-A310-F86AACED2A25", "com.GregWill.RiverCandy", 10564));
            BeaconRegions.Add(new BeaconRegion("B9407F30-F5F8-466E-AFF9-25556B57FE6D", "com.BenCelis.RiverBeetroot", 3505));
            BeaconRegions.Add(new BeaconRegion("702666BD-2369-48E9-AAE4-1022C95B0E8F", "com.BenCelis.RiverLemon", 19053));
            BeaconRegions.Add(new BeaconRegion("​3887E468-4EF1-4D31-8B5B-B2A900326D1B", "com.BenCelis.RiverCandy", 19350));
        }


        public override void RegionEntered(CLLocationManager manager, CLRegion region)
        {
            base.RegionEntered(manager, region);
            var location = (region as CLBeaconRegion).Major.Int16Value;

            if (location == Settings.CurrentLocation)
                return;

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