using System;
using System.Collections.Generic;
using RiverMobile.Models;
using RiverMobile.Services;

namespace RiverMobile.Droid.Services
{
    public class BeaconService : IBeaconService
    {
        readonly IMessageService messageService;

        public BeaconService(
            IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public void StartMonitoring(HashSet<BeaconRegion> beaconRegions)
        {
            throw new NotImplementedException();
        }

        public void StartRanging(HashSet<BeaconRegion> beaconRegions)
        {
            throw new NotImplementedException();
        }

        public void StopMonitoring(HashSet<BeaconRegion> beaconRegions)
        {
            throw new NotImplementedException();
        }

        public void StopRanging(HashSet<BeaconRegion> beaconRegions)
        {
            throw new NotImplementedException();
        }
    }
}
