using CoreLocation;
using Foundation;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;
using RiverMobile.Helpers;
using RiverMobile.iOS.Delegates;
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

        protected readonly CLLocationManager locationManager = new CLLocationManager();
        HashSet<BeaconRegion> monitoredBeaconRegions = new HashSet<BeaconRegion>();
        HashSet<BeaconRegion> rangedBeaconRegions = new HashSet<BeaconRegion>();

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

        public void StartMonitoring(HashSet<BeaconRegion> beaconRegions)
        {
            //TODO: figure out if this is really nessessary
            // i.e. what happens if you add a region that already exists?
            // this needs to be answered for all of these methods
            var newBeaconRegions = beaconRegions.Except(monitoredBeaconRegions);

            foreach (var beaconRegion in newBeaconRegions)
            {
                var clBeaconRegion = CLBeaconRegionFactory(beaconRegion);

                monitoredBeaconRegions.Add(beaconRegion);

                locationManager.StartMonitoring(clBeaconRegion);
            };
        }

        public void StopMonitoring(HashSet<BeaconRegion> beaconRegions)
        {
            var newBeaconRegions = beaconRegions.Intersect(monitoredBeaconRegions);

            foreach (var beaconRegion in newBeaconRegions)
            {
                var clBeaconRegion = CLBeaconRegionFactory(beaconRegion);

                locationManager.StartMonitoring(clBeaconRegion);

                monitoredBeaconRegions.Remove(beaconRegion);
            };
        }

        public void StartRanging(HashSet<BeaconRegion> beaconRegions)
        {
            var newBeaconRegions = beaconRegions.Except(rangedBeaconRegions);

            foreach (var beaconRegion in newBeaconRegions)
            {
                var clBeaconRegion = CLBeaconRegionFactory(beaconRegion);

                rangedBeaconRegions.Add(beaconRegion);

                locationManager.StartMonitoring(clBeaconRegion);
                locationManager.StartRangingBeacons(clBeaconRegion);
            }
        }

        public void StopRanging(HashSet<BeaconRegion> beaconRegions)
        {
            var removeBeaconRegions = beaconRegions.Intersect(rangedBeaconRegions);

            foreach (var beaconRegion in removeBeaconRegions)
            {
                var clBeaconRegion = CLBeaconRegionFactory(beaconRegion);

                locationManager.StopMonitoring(clBeaconRegion);
                locationManager.StopRangingBeacons(clBeaconRegion);

                rangedBeaconRegions.Remove(beaconRegion);
            }
        }

        void WireLocationManager()
        {
            locationManager.AuthorizationChanged += (sender, e) =>
            {
                Console.WriteLine($"The auth changed: {e.Status}");
            };

            locationManager.DeferredUpdatesFinished += (sender, e) =>
            {
                Console.WriteLine($"DeferredUpdatedFinished: {e.Error.Description}");
            };

            locationManager.DidStartMonitoringForRegion += (sender, e) =>
            {
                locationManager.RequestState(e.Region);
                Console.WriteLine($"DidStartMonitoringForRegion: {e.Region.Description}");
            };

            locationManager.DidDetermineState += (sender, e) =>
            {
                Console.WriteLine($"DidDetermineState: {e.State}");
            };

            locationManager.DidRangeBeacons += (sender, e) =>
            {
                Console.WriteLine($"DidRangeBeacons: {e.Beacons.FirstOrDefault()?.Minor}");
            };



            //locationManager.DidDetermineState += (sender, e) =>
            //{
            //    if (e.State == CLRegionState.Inside)
            //    {
            //        messageService.Subscribe(this, (object messenger, RecordStampMessage message) =>
            //        {
            //            PrintBeaconLocation(message.Stamp.Location);
            //        });

            //        locationManager.DidRangeBeacons += OnDidRangeBeacons;
            //    }
            //    else
            //    {
            //        messageService.Unsubscribe<RecordStampMessage>(this);
            //        //locationManager.DidRangeBeacons -= OnDidRangeBeacons;
            //    }
            //};

            locationManager.DidVisit += (sender, e) =>
            {
                Console.WriteLine($"DidVisit: {e.Visit.Description}");
            };

            locationManager.Failed += (sender, e) =>
            {
                Console.WriteLine($"Failed: {e.Error.Description}");
            };

            locationManager.LocationsUpdated += (sender, e) =>
            {
                Console.WriteLine($"LocationsUpdated: {e.Locations[e.Locations.Length-1]}");
            };

            locationManager.LocationUpdatesPaused += (sender, e) =>
            {
                Console.WriteLine($"LocationUpdatesPaused: {e.ToString()}");
            };

            locationManager.LocationUpdatesResumed += (sender, e) =>
            {
                Console.WriteLine($"LocationUpdatesResumed: {e.ToString()}");
            };

            locationManager.MonitoringFailed += (sender, e) =>
            {
                Console.WriteLine($"LocationUpdatesResumed: {e.Error.Description}");
            };
            
            locationManager.RangingBeaconsDidFailForRegion += (sender, e) =>
            {
                Console.WriteLine($"RangingBeaconsDidFailForRegion: {e.Error.Description}");
            };

            locationManager.RegionEntered += (sender, e) =>
            {
                Console.WriteLine($"Entered Region: {e.Region.Description}");
            };

            locationManager.RegionLeft += (sender, e) =>
            {
                Console.WriteLine($"Exited Region: {e.Region.Description}");
            };

            locationManager.UpdatedHeading += (sender, e) =>
            {
                Console.WriteLine($"Exited Region: {e.NewHeading.DebugDescription}");
            };

            locationManager.UpdatedLocation += (sender, e) =>
            {
                Console.WriteLine($"Exited Region: {e.NewLocation.DebugDescription}");
            };
        }

        void WireMessages()
        {
            messageService.Subscribe(this, (object messenger, DidEnterBackground message) =>
            {
                //TODO: this is a hack to get around backgrounding limitations on iOS
                //You need to define a 5-color-mapping scheme to get around this
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
                Personal = new Relationship<Personal>
                {
                    Data = JsonConvert.DeserializeObject<Personal>(Settings.UserJson, new JsonSerializerSettings())
                }
            };

            messageService.Send(new RecordStampMessage(stamp));
        }

        protected void PrintBeaconLocation(int beaconLocation)
        {
            Console.WriteLine($"First Beacon: {beaconLocation}");
        }

        CLBeaconRegion CLBeaconRegionFactory(BeaconRegion beaconRegion)
        {
            CLBeaconRegion clBeaconRegion;

            if (beaconRegion.Minor.HasValue)
                clBeaconRegion = new CLBeaconRegion(
                    new NSUuid(beaconRegion.Uuid),
                    beaconRegion.Major.Value,
                    beaconRegion.Minor.Value,
                    beaconRegion.Id);

            else if (beaconRegion.Major.HasValue)
                clBeaconRegion = new CLBeaconRegion(
                    new NSUuid(beaconRegion.Uuid),
                    beaconRegion.Major.Value,
                    beaconRegion.Id);

            else
                clBeaconRegion = new CLBeaconRegion(
                    new NSUuid(beaconRegion.Uuid),
                    beaconRegion.Id);

            clBeaconRegion.NotifyEntryStateOnDisplay = true;
            clBeaconRegion.NotifyOnEntry = true;
            clBeaconRegion.NotifyOnExit = true;

            return clBeaconRegion;
        }
    }
}
