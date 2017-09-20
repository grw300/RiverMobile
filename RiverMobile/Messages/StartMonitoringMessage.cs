using RiverMobile.Models;
using System;
using System.Collections.Generic;

namespace RiverMobile.Messages
{
    public class StartMonitoringMessage : IMessage
    {
        public readonly HashSet<BeaconRegion> BeaconRegions;

        public StartMonitoringMessage(HashSet<BeaconRegion> beaconRegions)
        {
            BeaconRegions = beaconRegions;
        }
    }
}
