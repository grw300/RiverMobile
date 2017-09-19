using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
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
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            var riverApp = new RiverApp();

            var riveriOSBootstrapper = new RiveriOSBootstrapper(riverApp);
            riveriOSBootstrapper.Run();

            LoadApplication(riverApp);

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            Console.WriteLine("App entering background state.");
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            Console.WriteLine("App will enter foreground");
        }
    }
}
