using RiverMobile.Models;
using System;
using System.Collections.Generic;

namespace RiverMobile.Messages
{
    public class StopMonitoringMessage : IMessage
    {
        public readonly HashSet<BeaconRegion> BeaconRegions;

        public StopMonitoringMessage(HashSet<BeaconRegion> beaconRegions)
        {
            BeaconRegions = beaconRegions;
        }
    }
}
