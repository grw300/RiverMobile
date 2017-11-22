using Autofac;
using CoreLocation;
using RiverMobile.iOS.Services;
using RiverMobile.Services;

namespace RiverMobile.iOS
{
    public class RiveriOSModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CLLocationManager>()
                   .SingleInstance();

            builder.RegisterType<NearestNeighbors>()
                    .As<INearestNeighbors>()
                    .SingleInstance();
               
            builder.RegisterType<BeaconService>()
                   .As<IBeaconService>()
                   .SingleInstance();

            builder.RegisterType<StampUploadService>()
                   .As<IStampUploadService>()
                   .SingleInstance();
        }
    }
}