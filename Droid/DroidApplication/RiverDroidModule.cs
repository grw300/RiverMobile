using Autofac;
using RiverMobile.Droid.Services;

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