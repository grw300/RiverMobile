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
        readonly IRiverAPIService riverAPIService;
        readonly INavigator navigator;
        readonly IViewFactory viewFactory;

        readonly Func<string, string, string> Icon = (platform, icon) =>
        {
            switch (platform)
            {
                case Device.iOS:
                    return icon;
                default:
                    return null;
            }
        };

        Lazy<MasterDetailPage> masterDetailPage;
        public MasterDetailPage MainPage
        {
            get
            {
                return masterDetailPage.Value;
            }
        }

        public LoginService(
            IRiverAPIService riverAPIService,
            INavigator navigator,
            IViewFactory viewFactory)
        {
            this.riverAPIService = riverAPIService;
            this.navigator = navigator;
            this.viewFactory = viewFactory;

            masterDetailPage = new Lazy<MasterDetailPage>(ConfigureMainPage);
        }

        public async Task LoginAsync(string UserName)
        {
            var users = await riverAPIService.GetRiverModelsAsync<Personal>($"?filter[name]={UserName}");
            var user = users.FirstOrDefault();
            Settings.UserName = user.Name;
            Settings.UserId = user.Id.ToString();

            Settings.IsLoggedIn = true;

            MainPage.IsPresented = false;
            Application.Current.MainPage = MainPage;
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

        MasterDetailPage ConfigureMainPage()
        {
            var mainPage = viewFactory.Resolve<MainViewModel>() as MasterDetailPage;

            var chatPage = viewFactory.Resolve<ChatViewModel>();
            var personnelPage = viewFactory.Resolve<PersonnelViewModel>();
            var reportPage = viewFactory.Resolve<ReportViewModel>();

            mainPage.Master = viewFactory.Resolve<SettingsViewModel>();


            mainPage.Detail = new TabbedPage
            {
                Children =
                    {
                        new NavigationPage(personnelPage)
                        {
                            Title = "Personnel",
                            Icon = "Personnel.png"
                        },
                        new NavigationPage(reportPage)
                        {
                            Title = "Report",
                            Icon = "Report.png"
                        },
                        new NavigationPage(chatPage)
                        {
                            Title = "Chat",
                            Icon = "Chat.png"
                        }
                    }
            };

            return mainPage;
        }
    }
}
