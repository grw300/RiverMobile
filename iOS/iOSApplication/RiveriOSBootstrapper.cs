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

            var beaconService = container.Resolve<IBeaconService>();
            var messageService = container.Resolve<IMessageService>();

            WireMessages(beaconService, messageService);
        }

        void WireMessages(IBeaconService beaconService, IMessageService messageService)
        {
            messageService.Subscribe(beaconService, (object messanger, StartRangingMessage message) =>
            {
                beaconService.StartRanging(message.BeaconRegions);
            });

            messageService.Subscribe(beaconService, (object messanger, StopRangingMessage message) =>
            {
                beaconService.StopRanging(message.BeaconRegions);
            });

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
