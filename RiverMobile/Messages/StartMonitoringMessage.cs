using System;
using System.Collections.Generic;

namespace RiverMobile.Messages
{
    public class StartMonitoringMessage : IMessage
    {
        public readonly List<(string uuid, string id)> BeaconRegions;

        public StartMonitoringMessage(List<(string uuid, string id)> beaconRegions)
        {
            BeaconRegions = beaconRegions;
        }
    }
}
