using System;
using System.Linq;
using System.Threading.Tasks;
using MobileCore.Factories;
using MobileCore.Interfaces;
using RiverMobile.Helpers;
using RiverMobile.Models;
using RiverMobile.ViewModels;
using Xamarin.Forms;

namespace RiverMobile.Services
{
    public class LoginService : ILoginService
    {
        readonly IRiverApiService riverApiService;
        readonly INavigator navigator;
        readonly IViewFactory viewFactory;



        public LoginService(
            IRiverApiService riverApiService,
            INavigator navigator,
            IViewFactory viewFactory)
        {
            this.riverApiService = riverApiService;
            this.navigator = navigator;
            this.viewFactory = viewFactory;
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

            Application.Current.MainPage = viewFactory.Resolve<LoginViewModel>();
        }

        public Personal Register()
        {
            throw new NotImplementedException();
        }
    }
}
