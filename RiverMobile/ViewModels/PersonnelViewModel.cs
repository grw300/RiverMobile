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
        bool isVisible;

        public string CurrentLocation
        {
            get => Settings.CurrentLocation;
            private set
            {
                if (Settings.CurrentLocation == value)
                    return;

                string currentLocation = string.Empty;
                SetProperty(ref currentLocation, value);
                Settings.CurrentLocation = currentLocation;
            }
        }

        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        public IEnumerable<PersonalViewModel> NearbyPersonnel
        {
            get => personnel;
            set => SetProperty(ref personnel, value);
        }

        public string PersonnelFilter
        { get; set; }
        public ImageSource SiteMap { get; set; }

        public ICommand ShowPersonnelSettingsCommand { get; private set; }

        public PersonnelViewModel(
            INavigator navigator,
            IRiverAPIService riverAPIService)
        {
            this.navigator = navigator;
            this.riverAPIService = riverAPIService;

            ShowPersonnelSettingsCommand = new Command(async () =>
                await ShowPersonnelSettingsAsync()
            );

            Title = "Personnel";

            CurrentLocation = "5";
        }

        public override void OnAppearing(object obj, EventArgs e)
        {
            if (!Application.Current.Properties.TryGetValue("PersonalName", out var personalName) &&
                string.IsNullOrEmpty(personalName as string))
            {
                isVisible = false;
                navigator.PushModalAsync<LoginViewModel>();
            }
        }

        public override void NavigatedTo()
        {
            isVisible = true;
        }

        Task ShowPersonnelSettingsAsync()
        {
            return navigator.PushAsync(new PersonnelSettingsViewModel());
        }
    }
}
