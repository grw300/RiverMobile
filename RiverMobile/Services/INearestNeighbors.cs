using RiverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiverMobile.Services
{
    public interface INearestNeighbors
    {
        Dictionary<BeaconRegion, HashSet<BeaconRegion>> Neighbors { get; }
        HashSet<BeaconRegion> BeaconRegions { get; }
        void RecordStamp(int location);
    }
}
