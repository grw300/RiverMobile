using RiverMobile.Models;
using System;
using System.Collections.Generic;

namespace RiverMobile.Messages
{
    public class StopRangingMessage : IMessage
    {
        public readonly HashSet<BeaconRegion> BeaconRegions;

        public StopRangingMessage(HashSet<BeaconRegion> beaconRegions)
        {
            BeaconRegions = beaconRegions;
        }
    }
}
