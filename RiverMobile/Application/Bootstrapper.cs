using MobileCore.Bootstrapping;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Autofac;
using MobileCore.Factories;
using RiverMobile.ViewModels;
using RiverMobile.Views;
using RiverMobile.Services;
using RiverMobile.Models;
using RiverMobile.Helpers;

namespace RiverMobile
{
    public class Bootstrapper : AutofacBootstrapper
    {
        readonly Application application;

        public Bootstrapper(Application application)
        {
            this.application = application;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<RiverAppModule>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            var viewFactory = container.Resolve<IViewFactory>();

            var loginService = container.Resolve<ILoginService>();

            application.Properties["RiverServiceBaseAddress"] = "http://river-api.azurewebsites.net/api/v1/";

            application.MainPage = Settings.IsLoggedIn ? loginService.MainPage
                                                       : viewFactory.Resolve<LoginViewModel>();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<SettingsViewModel, SettingsView>();
            viewFactory.Register<MainViewModel, MainView>();

            viewFactory.Register<ChatViewModel, ChatView>();
            viewFactory.Register<PersonnelViewModel, PersonnelView>();
            viewFactory.Register<ReportViewModel, ReportView>();
        }

        //MasterDetailPage CreateMasterDetailPage()
        //{
        //    var chatPage = viewFactory.Resolve<ChatViewModel>();
        //    var personnelPage = viewFactory.Resolve<PersonnelViewModel>();
        //    var reportPage = viewFactory.Resolve<ReportViewModel>();

        //    var settingsPage = viewFactory.Resolve<SettingsViewModel>();

        //    settingsPage.Icon = "Menu.png";

        //    var masterDetailPage = new MasterDetailPage
        //    {
        //        Master = settingsPage,
        //        Detail = new TabbedPage
        //        {
        //            Children =
        //            {
        //                new NavigationPage(personnelPage)
        //                {
        //                    Title = "Personnel",
        //                    Icon = Icon(Device.RuntimePlatform, "Personnel.png")
        //                },
        //                new NavigationPage(reportPage)
        //                {
        //                    Title = "Report",
        //                    Icon = Icon(Device.RuntimePlatform, "Report.png")
        //                },
        //                new NavigationPage(chatPage)
        //                {
        //                    Title = "Chat",
        //                    Icon = Icon(Device.RuntimePlatform, "Chat.png")
        //                }
        //            }
        //        }
        //    };

        //    return masterDetailPage;
        //}

        //MasterDetailPage CreateMasterDetailPage()
        //{
        //    var chatPage = viewFactory.Resolve<ChatViewModel>();
        //    var personnelPage = viewFactory.Resolve<PersonnelViewModel>();
        //    var reportPage = viewFactory.Resolve<ReportViewModel>();

        //    var settingsPage = viewFactory.Resolve<SettingsViewModel>();

        //    settingsPage.Icon = "Menu.png";

        //    var masterDetailPage = new MasterDetailPage
        //    {
        //        Master = settingsPage,
        //        Detail = new TabbedPage
        //        {
        //            Children =
        //            {
        //                new NavigationPage(personnelPage)
        //                {
        //                    Title = "Personnel",
        //                    Icon = Icon(Device.RuntimePlatform, "Personnel.png")
        //                },
        //                new NavigationPage(reportPage)
        //                {
        //                    Title = "Report",
        //                    Icon = Icon(Device.RuntimePlatform, "Report.png")
        //                },
        //                new NavigationPage(chatPage)
        //                {
        //                    Title = "Chat",
        //                    Icon = Icon(Device.RuntimePlatform, "Chat.png")
        //                }
        //            }
        //        }
        //    };

        //    return masterDetailPage;
        //}
    }
}
