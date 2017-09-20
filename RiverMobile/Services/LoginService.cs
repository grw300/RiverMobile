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
        readonly IMessageService messageService;
        readonly INavigator navigator;
        readonly IRiverApiService riverApiService;
        readonly IViewFactory viewFactory;

        readonly List<(string uuid, string id)> beaconRegions = new List<(string uuid, string id)>();

        public LoginService(
            IMessageService messageService,
            INavigator navigator,
            IRiverApiService riverApiService,
            IViewFactory viewFactory)
        {
            this.messageService = messageService;
            this.navigator = navigator;
            this.riverApiService = riverApiService;
            this.viewFactory = viewFactory;

            beaconRegions.Add((uuid: "B9407F30-F5F8-466E-AFF9-25556B57FE6D", id: "com.GregWill.RiverB9407F"));
            beaconRegions.Add((uuid: "CBE70FB5-6155-4D2D-BC3C-E9F4C2CB18E6", id: "com.GregWill.RiverCBE70F"));
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

            messageService.Send(new StopRangingMessage(beaconRegions));

            Application.Current.MainPage = viewFactory.Resolve<LoginViewModel>();
        }

        public Personal Register()
        {
            throw new NotImplementedException();
        }
    }
}
