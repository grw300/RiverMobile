﻿using System;
using System.Collections.Generic;

namespace RiverMobile.Messages
{
    public class StopRangingMessage : IMessage
    {
        public readonly List<(string uuid, string id)> BeaconRegions;

        public StopRangingMessage(List<(string uuid, string id)> beaconRegions)
        {
            BeaconRegions = beaconRegions;
        }
    }
}
