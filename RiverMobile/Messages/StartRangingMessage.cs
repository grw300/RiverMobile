using System;
using System.Collections.Generic;

namespace RiverMobile.Messages
{
    public class StartRangingMessage : IMessage
    {
        public readonly List<(string uuid, string id)> BeaconRegions;

        public StartRangingMessage(List<(string uuid, string id)> beaconRegions)
        {
            BeaconRegions = beaconRegions;
        }
    }
}
