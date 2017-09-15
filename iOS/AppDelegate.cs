using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using RiverMobile.iOS.Services;
using RiverMobile.Messages;
using RiverMobile.ViewModels;
using UIKit;
using Xamarin.Forms;

namespace RiverMobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        readonly static BeaconService beaconService = new BeaconService();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new RiverApp());

            WireUpBeaconRanging();

            return base.FinishedLaunching(app, options);
        }

        void WireUpBeaconRanging()
        {
            MessagingCenter.Subscribe<LoginViewModel, (string uuid, string id)>(this, MessangerKeys.StartRanging, (sender, args) =>
            {
                beaconService.StartRanging(args.uuid, args.id);
            });

            MessagingCenter.Subscribe<LoginViewModel, (string uuid, string id)>(this, MessangerKeys.StopRanging, (sender, args) =>
            {
                beaconService.StopRanging(args.uuid, args.id);
            });
        }
    }
}
