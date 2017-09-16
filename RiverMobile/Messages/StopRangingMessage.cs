using System;

namespace RiverMobile.Messages
{
    public class StopRangingMessage : IMessage
    {
        public readonly (string uuid, string id) BeaconRegion;

        public StopRangingMessage((string uuid, string id) beaconRegion)
        {
            BeaconRegion = beaconRegion;
        }
    }
}
