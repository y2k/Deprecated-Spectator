using Autofac;

namespace Spectator.iOS.Platform
{
    public class PlatformModule: Module
    {
        #region implemented abstract members of NinjectModule

        protected override void Load(ContainerBuilder builder)
        {
//            builder.RegisterType<BitmapImageDecoder>().As<ImageDecoder>();
//            builder.RegisterType<NotificationService>().As<INotificationService>();
        }

        #endregion
    }
}