using Autofac;
using RiverMobile.Droid.Services;
using RiverMobile.Services;

namespace RiverMobile.Droid
{
    public class RiverDroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BeaconService>()
                   .As<IBeaconService>()
                   .SingleInstance();
        }
    }
}