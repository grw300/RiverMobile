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
        readonly INearestNeighbors nearestNeighbors;
        readonly IRiverApiService riverApiService;
        readonly IStampBatchService stampBatchService;

        public MainViewModel(
            IBeaconService beaconService,
            IMessageService messageService,
            INearestNeighbors nearestNeighbors,
            IRiverApiService riverApiService,
            IStampBatchService stampBatchService)
        {
            this.beaconService = beaconService;
            this.messageService = messageService;
            this.nearestNeighbors = nearestNeighbors;
            this.riverApiService = riverApiService;
            this.stampBatchService = stampBatchService;

            Title = "Main";
        }

        public override void OnAppearing(object obj, EventArgs e)
        {
            beaconService.StartMonitoring(nearestNeighbors.BeaconRegions);
            beaconService.StartRanging(nearestNeighbors.BeaconRegions);
        }
    }
}
