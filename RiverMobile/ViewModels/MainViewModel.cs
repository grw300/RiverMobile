using MobileCore.ViewModels;
using RiverMobile.Messages;
using RiverMobile.Models;
using RiverMobile.Services;
using System;
using System.Collections.Generic;

namespace RiverMobile.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        readonly IBeaconService beaconService;
        readonly IMessageService messageService;
        readonly IRiverApiService riverApiService;
        readonly IStampBatchService stampBatchService;
        readonly HashSet<BeaconRegion> beaconRegions = new HashSet<BeaconRegion>();

        public MainViewModel(
            IBeaconService beaconService,
            IMessageService messageService,
            IRiverApiService riverApiService,
            IStampBatchService stampBatchService)
        {
            this.beaconService = beaconService;
            this.messageService = messageService;
            this.riverApiService = riverApiService;
            this.stampBatchService = stampBatchService;

            //TODO: fix this hack - you should be gettings these values from the API.
            //TODO: You need to centralize these GUIDs - there will be more than one.
            beaconRegions.Add(
                new BeaconRegion("B9407F30-F5F8-466E-AFF9-25556B57FE6D",
                                 "com.GregWill.RiverB9407F"));
            //beaconRegions.Add((uuid: "CBE70FB5-6155-4D2D-BC3C-E9F4C2CB18E6", id: "com.GregWill.RiverCBE70F"));
            Title = "Main";
        }

        public override void OnAppearing(object obj, EventArgs e)
        {
            beaconService.StartMonitoring(beaconRegions);
            beaconService.StartRanging(beaconRegions);
        }
    }
}
