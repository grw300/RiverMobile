using System;
using Autofac;
using RiverMobile.iOS.Services;
using RiverMobile.Messages;
using RiverMobile.Services;
using UIKit;
using Xamarin.Forms;

namespace RiverMobile.iOS
{
    public class RiveriOSBootstrapper : RiverAppBootstrapper
    {
        public RiveriOSBootstrapper(Xamarin.Forms.Application application)
            : base(application)
        { }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<RiveriOSModule>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            base.ConfigureApplication(container);

            var messageService = container.Resolve<IMessageService>();

            WireMessages(messageService);
        }

        void WireMessages(IMessageService messageService)
        {
            UIApplication.Notifications.ObserveDidEnterBackground((sender, e) =>
            {
                messageService.Send(new DidEnterBackground());
            });

            UIApplication.Notifications.ObserveDidBecomeActive((sender, args) =>
            {
                messageService.Send(new DidBecomeActive());
            });
        }
    }
}
