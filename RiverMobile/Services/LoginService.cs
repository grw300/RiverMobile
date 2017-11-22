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
using Newtonsoft.Json;

namespace RiverMobile.Services
{
    public class LoginService : ILoginService
    {
        readonly IBeaconService beaconService;
        readonly IMessageService messageService;
        readonly INavigator navigator;
        readonly INearestNeighbors nearestNeighbors;
        readonly IRiverApiService riverApiService;
        readonly IViewFactory viewFactory;


        public LoginService(
            IBeaconService beaconService,
            IMessageService messageService,
            INavigator navigator,
            INearestNeighbors nearestNeighbors,
            IRiverApiService riverApiService,
            IViewFactory viewFactory)
        {
            this.beaconService = beaconService;
            this.messageService = messageService;
            this.navigator = navigator;
            this.nearestNeighbors = nearestNeighbors;
            this.riverApiService = riverApiService;
            this.viewFactory = viewFactory;
        }

        public async Task LoginAsync(string UserName)
        {
            var users = await riverApiService.GetRiverModelsAsync<Personal>($"?filter[name]={UserName}");
            var user = users.FirstOrDefault();
            Settings.UserName = user.Name;
            Settings.UserId = user.Id.GetValueOrDefault();
            Settings.UserJson = JsonConvert.SerializeObject(user, new JsonSerializerSettings());

            Settings.IsLoggedIn = true;

            RiverApp.MainView.IsPresented = false;
            Application.Current.MainPage = RiverApp.MainView;
            await navigator.PopToRootAsync();
        }

        public void LogoutAsync()
        {
            Settings.IsLoggedIn = false;

            beaconService.StopRanging(nearestNeighbors.BeaconRegions);

            Application.Current.MainPage = viewFactory.Resolve<LoginViewModel>();
        }

        public Personal Register()
        {
            throw new NotImplementedException();
        }
    }
}
