using MobileCore.Interfaces;
using MobileCore.ViewModels;
using RiverMobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using RiverMobile.Helpers;
using RiverMobile.Messages;

namespace RiverMobile.ViewModels
{
    public class PersonnelViewModel : ViewModelBase
    {
        readonly IMessageService messageService;
        readonly INavigator navigator;
        readonly IRiverApiService riverApiService;

        int currentLocation = Settings.CurrentLocation;
        IEnumerable<PersonalViewModel> personnel;

        public int CurrentLocation
        {
            get => currentLocation;
            private set => SetProperty(ref currentLocation, value);
        }

        public IEnumerable<PersonalViewModel> NearbyPersonnel
        {
            get => personnel;
            set => SetProperty(ref personnel, value);
        }

        public string PersonnelFilter
        { get; set; }
        public ImageSource SiteMap { get; set; }

        public PersonnelViewModel(
            IMessageService messageService,
            INavigator navigator,
            IRiverApiService riverApiService)
        {
            this.messageService = messageService;
            this.navigator = navigator;
            this.riverApiService = riverApiService;

            WireMessages();

            Title = "Personnel";
        }

        void WireMessages()
        {
            messageService.Subscribe(this, (object messenger, DidEnterBackground message) =>
            {
                messageService.Unsubscribe<RecordStampMessage>(this);
            });

            messageService.Subscribe(this, (object messenger, DidBecomeActive message) =>
            {
                messageService.Subscribe(this, (object nestedMessenger, RecordStampMessage nestedMessage) =>
                {
                    CurrentLocation = nestedMessage.Stamp.Location;
                });
            });
        }

        //public override void OnAppearing(object obj, EventArgs e)
        //{
        //    messageService.Subscribe(this, (object messenger, RecordStampMessage message) =>
        //    {
        //        CurrentLocation = message.Stamp.Location;
        //    });
        //}

        //public override void OnDisappearing(object obj, EventArgs e)
        //{
        //    messageService.Unsubscribe<RecordStampMessage>(this);
        //}
    }
}
