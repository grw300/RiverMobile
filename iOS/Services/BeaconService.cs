using CoreLocation;
using Foundation;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;
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

        protected readonly CLLocationManager locationManager;
        HashSet<BeaconRegion> monitoredBeaconRegions = new HashSet<BeaconRegion>();
        HashSet<BeaconRegion> rangedBeaconRegions = new HashSet<BeaconRegion>();

        public BeaconService(
            CLLocationManager locationManager,
            IMessageService messageService,
            ICLLocationManagerDelegate strategy)
        {
            this.locationManager = locationManager;
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

            locationManager.Delegate = strategy;

            //WireLocationManager();
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

        void WireMessages()
        {
            messageService.Subscribe(this, (object messenger, DidEnterBackground message) =>
            {
                //TODO: this is a hack to get around backgrounding limitations on iOS
                //You need to define a nearest neighbors scheme and attach a new delegate
                locationManager.StartUpdatingLocation();
            });

            messageService.Subscribe(this, (object messenger, DidBecomeActive message) =>
            {
                locationManager.StopUpdatingLocation();
            });
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
