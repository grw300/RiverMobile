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

namespace RiverMobile.ViewModels
{
    public class PersonnelViewModel : ViewModelBase
    {
        readonly INavigator navigator;
        readonly IRiverAPIService riverAPIService;

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
            INavigator navigator,
            IRiverAPIService riverAPIService)
        {
            this.navigator = navigator;
            this.riverAPIService = riverAPIService;

            Title = "Personnel";

            CurrentLocation = 5;
        }
    }
}
