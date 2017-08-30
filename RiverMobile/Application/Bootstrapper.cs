using MobileCore.Bootstrapping;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Autofac;
using MobileCore.Factories;
using RiverMobile.ViewModels;
using RiverMobile.Views;

namespace RiverMobile
{
    public class Bootstrapper : AutofacBootstrapper
    {
        readonly Application application;
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
            application.Properties["RiverServiceBaseAddress"] = "http://river-api.azurewebsites.net/api/v1/";

            var viewFactory = container.Resolve<IViewFactory>();

            var chatPage = viewFactory.Resolve<ChatViewModel>();
            var personnelPage = viewFactory.Resolve<PersonnelViewModel>();
            var reportPage = viewFactory.Resolve<ReportViewModel>();

            var tabbedPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(personnelPage)
                    {
                        Title = "Personnel",
                        Icon = Icon(Device.RuntimePlatform, "Personnel.png")
                    },
                    new NavigationPage(reportPage)
                    {
                        Title = "Report",
                        Icon = Icon(Device.RuntimePlatform, "Report.png")
                    },
                    new NavigationPage(chatPage)
                    {
                        Title = "Chat",
                        Icon = Icon(Device.RuntimePlatform, "Chat.png")
                    }
                }
            };

            application.MainPage = tabbedPage;
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<ChatViewModel, ChatView>();
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<PersonnelViewModel, PersonnelView>();
            viewFactory.Register<ReportViewModel, ReportView>();

            viewFactory.Register<PersonnelSettingsViewModel, PersonnelSettingsView>();
        }
    }
}
