using RiverMobile.Models;
using System;
using System.Collections.Generic;

namespace RiverMobile.Messages
{
    public class StartRangingMessage : IMessage
    {
        public readonly HashSet<BeaconRegion> BeaconRegions;

        public StartRangingMessage(HashSet<BeaconRegion> beaconRegions)
        {
            BeaconRegions = beaconRegions;
        }
    }
}
