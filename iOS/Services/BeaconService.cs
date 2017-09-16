using System;
using System.Collections.Generic;
using System.Linq;
using CoreLocation;
using Foundation;
using RiverMobile.Helpers;
using RiverMobile.Messages;
using RiverMobile.Models;
using RiverMobile.Services;
using UIKit;

namespace RiverMobile.iOS.Services
{
    public class BeaconService : IBeaconService
    {
        readonly IMessageService messageService;

        readonly CLLocationManager locationManager = new CLLocationManager();
        IList<CLBeaconRegion> clBeaconRegions = new List<CLBeaconRegion>();

        public BeaconService(
            IMessageService messageService)
        {
            this.messageService = messageService;

            locationManager.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locationManager.RequestAlwaysAuthorization(); // works in background
                //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            // iOS 9 requires the following for background location updates
            // By default this is set to false and will not allow background updates
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locationManager.AllowsBackgroundLocationUpdates = true;
            }

            locationManager.RegionEntered += (sender, e) =>
            {
                messageService.Subscribe(this, (object messenger, RecordStampMessage message) =>
                {
                    PrintBeaconLocation(message.Stamp.Location);
                });

                locationManager.DidRangeBeacons += OnDidRangeBeacons;
            };

            locationManager.RegionLeft += (sender, e) =>
            {
                locationManager.DidRangeBeacons -= OnDidRangeBeacons;
            };
        }

        public void StartRanging((string uuid, string id) beaconRegion)
        {
            var beaconUUID = new NSUuid(beaconRegion.uuid);

            var clBeaconRegion = new CLBeaconRegion(beaconUUID, beaconRegion.id)
            {
                NotifyEntryStateOnDisplay = true,
                NotifyOnEntry = true,
                NotifyOnExit = true
            };

            clBeaconRegions.Add(clBeaconRegion);

            locationManager.StartMonitoring(clBeaconRegion);
            locationManager.StartRangingBeacons(clBeaconRegion);
        }

        public void StopRanging((string uuid, string id) beaconRegion)
        {
            var clBeaconRegion = clBeaconRegions
                .FirstOrDefault(
                    region =>
                        region.ProximityUuid.AsString() == beaconRegion.uuid &&
                        region.Identifier == beaconRegion.id
                    );

            locationManager.StopMonitoring(clBeaconRegion);
            locationManager.StopRangingBeacons(clBeaconRegion);

            clBeaconRegions.Remove(clBeaconRegion);
        }

        void OnDidRangeBeacons(object sender, CLRegionBeaconsRangedEventArgs e)
        {
            var firstBeacon = e.Beacons.FirstOrDefault();
            var location = firstBeacon.Minor.Int32Value;

            if (location == Settings.CurrentLocation)
                return;

            var stamp = new Stamp
            {
                Time = DateTime.UtcNow,
                Location = location,
                PersonalId = Settings.UserId
            };

            messageService.Send(new RecordStampMessage(stamp));
        }

        protected void PrintBeaconLocation(int beaconLocation)
        {
            Console.WriteLine($"First Beacon: {beaconLocation}");
        }
    }
}
