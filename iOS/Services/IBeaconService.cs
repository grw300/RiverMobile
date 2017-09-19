using System.Collections.Generic;

namespace RiverMobile.iOS.Services
{
    public interface IBeaconService
    {
        void StartMonitoring(List<(string uuid, string id)> beaconRegions);

        void StopMonitoring(List<(string uuid, string id)> beaconRegions);

        void StartRanging(List<(string uuid, string id)> beaconRegions);

        void StopRanging(List<(string uuid, string id)> beaconRegions);
    }
}