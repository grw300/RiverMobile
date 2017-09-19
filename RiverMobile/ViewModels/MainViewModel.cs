using System;
using System.Collections.Generic;
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

        List<(string uuid, string id)> beaconRegions = new List<(string uuid, string id)>();

        public MainViewModel(
            IMessageService messageService,
            IRiverApiService riverApiService)
        {
            this.messageService = messageService;
            this.riverApiService = riverApiService;

            beaconRegions.Add((uuid: "B9407F30-F5F8-466E-AFF9-25556B57FE6D", id: "com.GregWill.RiverB9407F"));
            beaconRegions.Add((uuid: "CBE70FB5-6155-4D2D-BC3C-E9F4C2CB18E6", id: "com.GregWill.RiverCBE70F"));
            Title = "Main";
        }

        public override void OnAppearing(object obj, EventArgs e)
        {
            messageService.Send(new StartRangingMessage(beaconRegions));
        }
    }
}
