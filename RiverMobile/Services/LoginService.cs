using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileCore.Factories;
using MobileCore.Interfaces;
using RiverMobile.Helpers;
using RiverMobile.Messages;
using RiverMobile.Models;
using RiverMobile.ViewModels;
using Xamarin.Forms;

namespace RiverMobile.Services
{
    public class LoginService : ILoginService
    {
        readonly IBeaconService beaconService;
        readonly IMessageService messageService;
        readonly INavigator navigator;
        readonly IRiverApiService riverApiService;
        readonly IViewFactory viewFactory;

        readonly HashSet<BeaconRegion> beaconRegions = new HashSet<BeaconRegion>();

        public LoginService(
            IBeaconService beaconService,
            IMessageService messageService,
            INavigator navigator,
            IRiverApiService riverApiService,
            IViewFactory viewFactory)
        {
            this.beaconService = beaconService;
            this.messageService = messageService;
            this.navigator = navigator;
            this.riverApiService = riverApiService;
            this.viewFactory = viewFactory;

            //TODO: fix this hack - you should be gettings these values from the API.
            //TODO: You need to centralize these GUIDs - there will be more than one.
            beaconRegions.Add(
                new BeaconRegion("B9407F30-F5F8-466E-AFF9-25556B57FE6D",
                                 "com.GregWill.RiverB9407F"));

            //beaconRegions.Add(
            //new BeaconRegion("CBE70FB5-6155-4D2D-BC3C-E9F4C2CB18E6",
            //"com.GregWill.RiverCBE70F"));
        }

        public async Task LoginAsync(string UserName)
        {
            var users = await riverApiService.GetRiverModelsAsync<Personal>($"?filter[name]={UserName}");
            var user = users.FirstOrDefault();
            Settings.UserName = user.Name;
            Settings.UserId = user.Id.GetValueOrDefault();

            Settings.IsLoggedIn = true;

            RiverApp.MainView.IsPresented = false;
            Application.Current.MainPage = RiverApp.MainView;
            await navigator.PopToRootAsync();
        }

        public void LogoutAsync()
        {
            Settings.IsLoggedIn = false;

            beaconService.StopRanging(beaconRegions);

            Application.Current.MainPage = viewFactory.Resolve<LoginViewModel>();
        }

        public Personal Register()
        {
            throw new NotImplementedException();
        }
    }
}
