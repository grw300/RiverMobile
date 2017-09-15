using Autofac;
using RiverMobile.Services;
using RiverMobile.ViewModels;
using RiverMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using JsonApiSerializer;
using Newtonsoft.Json;

namespace RiverMobile
{
    public class RiverAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RiverAPIService>()
                   .As<IRiverAPIService>()
                   .SingleInstance();
            builder.RegisterType<LoginService>()
                   .As<ILoginService>()
                   .SingleInstance();

            builder.RegisterType<ChatViewModel>()
                   .SingleInstance();
            builder.RegisterType<LoginViewModel>()
                   .SingleInstance();
            builder.RegisterType<PersonnelViewModel>()
                   .SingleInstance();
            builder.RegisterType<ReportViewModel>()
                   .SingleInstance();

            builder.RegisterType<MainViewModel>()
                   .SingleInstance();
            builder.RegisterType<SettingsViewModel>()
                   .SingleInstance();

            builder.RegisterType<ChatView>()
                   .SingleInstance();
            builder.RegisterType<LoginView>()
                   .SingleInstance();
            builder.RegisterType<PersonnelView>()
                   .SingleInstance();
            builder.RegisterType<ReportView>()
                   .SingleInstance();

            builder.RegisterType<MainView>()
                   .SingleInstance();
            builder.RegisterType<SettingsView>()
                   .SingleInstance();

            //builder.RegisterInstance<Func<Page>>(() =>
            //    ((TabbedPage)Application.Current.MainPage).CurrentPage
            //);
        }
    }
}
