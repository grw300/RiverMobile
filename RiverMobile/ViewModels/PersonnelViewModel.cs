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

        IEnumerable<PersonalViewModel> personnel;

        public int CurrentLocation
        {
            get => Settings.CurrentLocation;
            private set
            {
                if (Settings.CurrentLocation == value)
                    return;

                int currentLocation = -1;
                SetProperty(ref currentLocation, value);
                Settings.CurrentLocation = currentLocation;
            }
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

            Title = "Personnel";
        }

        public override void OnAppearing(object obj, EventArgs e)
        {
            messageService.Subscribe(this, (object messenger, RecordStampMessage message) =>
            {
                CurrentLocation = message.Stamp.Location;
            });
        }

        public override void OnDisappearing(object obj, EventArgs e)
        {
            messageService.Unsubscribe<RecordStampMessage>(this);
        }
    }
}
