using System;
namespace RiverMobile.Droid.Services
{
    public interface IBeaconService
    {
        void StartRanging(BeaconRegion beaconRegion);

        void StopRanging(BeaconRegion beaconRegion);
    }
}
