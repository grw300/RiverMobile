using System;
using Autofac;
using RiverMobile.Messages;
using RiverMobile.Services;
using Xamarin.Forms;
using RiverMobile.Droid.Services;

namespace RiverMobile.Droid
{
    public class RiverDroidBootstrapper : RiverAppBootstrapper
    {
        public RiverDroidBootstrapper(Xamarin.Forms.Application application)
            : base(application)
        { }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<RiverDroidModule>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            base.ConfigureApplication(container);

            //var beaconService = container.Resolve<IBeaconService>();
            //var messageService = container.Resolve<IMessageService>();

            //messageService.Subscribe(beaconService, (object messanger, StartRangingMessage message) =>
            //{
            //    beaconService.StartRanging(message.BeaconRegion);
            //});

            //messageService.Subscribe(beaconService, (object messanger, StopRangingMessage message) =>
            //{
            //    beaconService.StopRanging(message.BeaconRegion);
            //});
        }
    }
}
