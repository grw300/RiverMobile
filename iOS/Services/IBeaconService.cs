namespace RiverMobile.iOS.Services
{
    public interface IBeaconService
    {
        void StartRanging((string uuid, string id) beaconRegion);

        void StopRanging((string uuid, string id) beaconRegion);
    }
}