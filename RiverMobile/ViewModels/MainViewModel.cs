using System;
using MobileCore.ViewModels;
using RiverMobile.Messages;
using RiverMobile.Services;
using Xamarin.Forms;

namespace RiverMobile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        readonly IMessageService messageService;
        readonly IRiverApiService riverApiService;

        (string uuid, string id) beaconRegion;

        public MainViewModel(
            IMessageService messageService,
            IRiverApiService riverApiService)
        {
            this.messageService = messageService;
            this.riverApiService = riverApiService;

            beaconRegion = (uuid: "B9407F30-F5F8-466E-AFF9-25556B57FE6D", id: "com.GregWill.River");
            Title = "Main";
        }

        public override void OnAppearing(object obj, EventArgs e)
        {
            messageService.Send(new StartRangingMessage(beaconRegion));
        }
    }
}
