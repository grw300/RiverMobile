using RiverMobile.Models;
using System.Collections.Generic;

namespace RiverMobile.iOS.Services
{
    public interface IBeaconService
    {
        void StartMonitoring(HashSet<BeaconRegion> beaconRegions);

        void StopMonitoring(HashSet<BeaconRegion> beaconRegions);

        void StartRanging(HashSet<BeaconRegion> beaconRegions);

        void StopRanging(HashSet<BeaconRegion> beaconRegions);
    }
}