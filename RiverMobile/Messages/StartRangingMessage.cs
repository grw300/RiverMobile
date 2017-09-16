using System;

namespace RiverMobile.Messages
{
    public class StartRangingMessage : IMessage
    {
        public readonly (string uuid, string id) BeaconRegion;

        public StartRangingMessage((string uuid, string id) beaconRegion)
        {
            BeaconRegion = beaconRegion;
        }
    }
}
