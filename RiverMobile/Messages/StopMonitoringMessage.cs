using System;
using System.Collections.Generic;

namespace RiverMobile.Messages
{
    public class StopMonitoringMessage : IMessage
    {
        public readonly List<(string uuid, string id)> BeaconRegions;

        public StopMonitoringMessage(List<(string uuid, string id)> beaconRegions)
        {
            BeaconRegions = beaconRegions;
        }
    }
}
