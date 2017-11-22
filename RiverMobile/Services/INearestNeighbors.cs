using RiverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiverMobile.Services
{
    public interface INearestNeighbors
    {
        HashSet<BeaconRegion> BeaconRegions { get; }
    }
}
