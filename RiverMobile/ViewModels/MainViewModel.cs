using System;
using MobileCore.ViewModels;
using RiverMobile.Messages;
using RiverMobile.Services;
using Xamarin.Forms;

namespace RiverMobile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        readonly IRiverAPIService riverAPIService;

        public MainViewModel(
            IRiverAPIService riverAPIService)
        {
            this.riverAPIService = riverAPIService;

            Title = "Main";
        }

        public async override void OnAppearing(object obj, EventArgs e)
        {
            var beaconRegion = (uuid: "B9407F30-F5F8-466E-AFF9-25556B57FE6D", id: "com.GregWill.River");
            MessagingCenter.Send(this, MessangerKeys.StartRanging, beaconRegion);
        }

        public override void OnDisappearing(object obj, EventArgs e)
        {
            var beaconRegion = (uuid: "B9407F30-F5F8-466E-AFF9-25556B57FE6D", id: "com.GregWill.River");
            MessagingCenter.Send(this, MessangerKeys.StopRanging, beaconRegion);
        }
    }
}
