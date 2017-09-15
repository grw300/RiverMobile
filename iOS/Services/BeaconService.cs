using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using JsonApiSerializer;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;
using RiverMobile.Helpers;
using RiverMobile.Messages;
using RiverMobile.Models;
using UIKit;
using Xamarin.Forms;

namespace RiverMobile.iOS.Services
{
    public class BeaconService
    {
        readonly CLLocationManager locationManager = new CLLocationManager();
        IList<CLBeaconRegion> beaconRegions = new List<CLBeaconRegion>();

        public BeaconService()
        {
            locationManager.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locationManager.RequestAlwaysAuthorization(); // works in background
                                                              //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            // iOS 9 requires the following for background location updates
            // By default this is set to false and will not allow background updates
            locationManager.AllowsBackgroundLocationUpdates |= UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

            locationManager.RegionEntered += (sender, e) =>
            {
                MessagingCenter.Subscribe<BeaconService, string>(this, MessangerKeys.RecordStamp, (who, args) =>
                {
                    PrintBeacon(args);
                });

                locationManager.DidRangeBeacons += OnDidRangeBeacons;
            };

            locationManager.RegionLeft += (sender, e) =>
            {
                locationManager.DidRangeBeacons -= OnDidRangeBeacons;
            };
        }

        public void StartRanging(string uuid, string id)
        {
            var beaconUUID = new NSUuid(uuid);

            var beaconRegion = new CLBeaconRegion(beaconUUID, id)
            {
                NotifyEntryStateOnDisplay = true,
                NotifyOnEntry = true,
                NotifyOnExit = true
            };

            beaconRegions.Add(beaconRegion);

            locationManager.StartMonitoring(beaconRegion);
            locationManager.StartRangingBeacons(beaconRegion);
        }

        public void StopRanging(string uuid, string id)
        {
            var beaconRegion = beaconRegions
                .FirstOrDefault(
                    region =>
                        region.ProximityUuid.AsString() == uuid &&
                        region.Identifier == id
                    );

            locationManager.StopMonitoring(beaconRegion);
            locationManager.StopRangingBeacons(beaconRegion);

            beaconRegions.Remove(beaconRegion);
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
                PersonalId = Guid.Parse(Settings.UserId)
            };

            var stampString = JsonConvert.SerializeObject(stamp, new JsonApiSerializerSettings());

            MessagingCenter.Send(this, MessangerKeys.RecordStamp, stampString);
        }

        protected void PrintBeacon(string stampString)
        {
            var stamp = JsonConvert.DeserializeObject<Stamp>(stampString, new JsonApiSerializerSettings());
            Console.WriteLine($"First Beacon: {stamp.Location}");
        }
    }
}
