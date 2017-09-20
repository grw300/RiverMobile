using CoreLocation;
using Foundation;
using RiverMobile.Helpers;
using RiverMobile.Messages;
using RiverMobile.Models;
using RiverMobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace RiverMobile.iOS.Services
{
    public class BeaconService : IBeaconService
    {
        readonly IMessageService messageService;

        readonly static CLLocationManager locationManager = new CLLocationManager();
        HashSet<(string uuid, string id)> beaconRegionsSet = new HashSet<(string uuid, string id)>();

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

            WireLocationManager();
            WireMessages();
        }


        public void StartMonitoring(List<(string uuid, string id)> beaconRegions)
        {
            throw new NotImplementedException();
        }

        public void StopMonitoring(List<(string uuid, string id)> beaconRegions)
        {
            throw new NotImplementedException();
        }

        public void StartRanging(List<(string uuid, string id)> beaconRegions)
        {
            var newBeaconRegions = beaconRegions.Except(beaconRegionsSet);

            foreach (var beaconRegion in newBeaconRegions)
            {
                var clBeaconRegion = CLBeaconRegionFactory(beaconRegion);

                beaconRegionsSet.Add(beaconRegion);

                locationManager.StartMonitoring(clBeaconRegion);
                locationManager.StartRangingBeacons(clBeaconRegion);
            }
        }

        public void StopRanging(List<(string uuid, string id)> beaconRegions)
        {
            var removeBeaconRegions = beaconRegions.Intersect(beaconRegionsSet);

            foreach (var beaconRegion in removeBeaconRegions)
            {
                var clBeaconRegion = CLBeaconRegionFactory(beaconRegion);

                locationManager.StopMonitoring(clBeaconRegion);
                locationManager.StopRangingBeacons(clBeaconRegion);

                beaconRegionsSet.Remove(beaconRegion);
            }
        }

        CLBeaconRegion CLBeaconRegionFactory((string uuid, string id) beaconRegion)
        {
            var beaconUUID = new NSUuid(beaconRegion.uuid);

            return new CLBeaconRegion(beaconUUID, beaconRegion.id)
            {
                NotifyEntryStateOnDisplay = true,
                NotifyOnEntry = true,
                NotifyOnExit = true
            };
        }

        void WireLocationManager()
        {
            locationManager.DidStartMonitoringForRegion += (sender, e) =>
            {
                locationManager.RequestState(e.Region);
            };

            locationManager.DidDetermineState += (sender, e) =>
            {
                if (e.State == CLRegionState.Inside)
                {
                    messageService.Subscribe(this, (object messenger, RecordStampMessage message) =>
                    {
                        PrintBeaconLocation(message.Stamp.Location);
                    });

                    locationManager.DidRangeBeacons += OnDidRangeBeacons;
                }
                else
                {
                    messageService.Unsubscribe<RecordStampMessage>(this);
                    //locationManager.DidRangeBeacons -= OnDidRangeBeacons;
                }
            };

            locationManager.RegionEntered += (sender, e) =>
            {
                Console.WriteLine($"Entered Region: {e.Region.Description}");
            };

            locationManager.RegionLeft += (sender, e) =>
            {
                Console.WriteLine($"Exited Region: {e.Region.Description}");
            };
        }

        void WireMessages()
        {
            messageService.Subscribe(this, (object messenger, DidEnterBackground message) =>
            {
                locationManager.StartUpdatingLocation();
            });

            messageService.Subscribe(this, (object messenger, DidBecomeActive message) =>
            {
                locationManager.StopUpdatingLocation();
            });
        }

        void OnDidRangeBeacons(object sender, CLRegionBeaconsRangedEventArgs e)
        {
            var firstBeacon = e.Beacons.FirstOrDefault();
            var location = firstBeacon?.Minor.Int32Value ?? -1;

            if (location == Settings.CurrentLocation)
                return;

            Settings.CurrentLocation = location;

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
