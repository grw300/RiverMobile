using System;
using Autofac;
using MobileCore.Bootstrapping;
using MobileCore.Factories;
using RiverMobile.Helpers;
using RiverMobile.Services;
using RiverMobile.ViewModels;
using RiverMobile.Views;
using Xamarin.Forms;

namespace RiverMobile
{
    public class RiverAppBootstrapper : AutofacBootstrapper
    {
        protected readonly Application application;

        public RiverAppBootstrapper(Application application)
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

            RiverApp.mainView = new Lazy<MasterDetailPage>(() => ConfigureMainView(viewFactory));

            application.MainPage = Settings.IsLoggedIn ? RiverApp.MainView
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

        MasterDetailPage ConfigureMainView(IViewFactory viewFactory)
        {
            var mainView = viewFactory.Resolve<MainViewModel>() as MasterDetailPage;

            var chatView = viewFactory.Resolve<ChatViewModel>();
            var personnelView = viewFactory.Resolve<PersonnelViewModel>();
            var reportView = viewFactory.Resolve<ReportViewModel>();

            mainView.Master = viewFactory.Resolve<SettingsViewModel>();

            mainView.Detail = new TabbedPage
            {
                Children =
                    {
                        new NavigationPage(personnelView)
                        {
                            Title = "Personnel",
                            Icon = "Personnel.png"
                        },
                        new NavigationPage(reportView)
                        {
                            Title = "Report",
                            Icon = "Report.png"
                        },
                        new NavigationPage(chatView)
                        {
                            Title = "Chat",
                            Icon = "Chat.png"
                        }
                    }
            };

            return mainView;
        }
    }
}
