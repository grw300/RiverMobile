using System;
using RiverMobile.Services;

namespace RiverMobile.Droid.Services
{
    public class BeaconService : IBeaconService
    {
        readonly IMessageService messageService;

        public BeaconService(
            IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public void StartRanging((string uuid, string id) beaconRegion)
        {
            throw new NotImplementedException();
        }

        public void StopRanging((string uuid, string id) beaconRegion)
        {
            throw new NotImplementedException();
        }
    }
}
