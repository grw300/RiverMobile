using Autofac;
using RiverMobile.iOS.Services;

namespace RiverMobile.iOS
{
    public class RiveriOSModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BeaconService>()
                   .As<IBeaconService>()
                   .SingleInstance();
        }
    }
}