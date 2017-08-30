using RiverMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RiverMobile
{
    public partial class RiverApp : Application
    {
        public RiverApp()
        {
            InitializeComponent();

            var bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();
        }

        //public static void SetMainPage()
        //{
        //    // Device.OnPlatform(T, T, T) is deprecated; this is the workaround
        //    // to get the correct icon on iOS and return null for all others.
        //    Func<string, string, string> runtimeIcon = (platform, icon) =>
        //    {
        //        switch (platform)
        //        {
        //            case Device.iOS:
        //                return icon;
        //            default:
        //                return null;
        //        }
        //    };

        //    Current.MainPage = new TabbedPage
        //    {
        //        Children =
        //        {
        //            new NavigationPage(new ItemsPage())
        //            {
        //                Title = "Browse",
        //                Icon = runtimeIcon(Device.RuntimePlatform, "tab_feed.png")
        //            },
        //            new NavigationPage(new AboutPage())
        //            {
        //                Title = "About",
        //                Icon = runtimeIcon(Device.RuntimePlatform, "tab_about.png")
        //            },
        //        }
        //    };
        //}
    }
}
