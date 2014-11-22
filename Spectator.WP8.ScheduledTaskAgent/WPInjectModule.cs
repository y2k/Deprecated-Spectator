using Autofac;
using Spectator.Core.Model.Push;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WindowsPhone8;
using XamarinCommons.Image;

namespace Spectator.WP8.ScheduledTaskAgent
{
    public class WPInjectModule : Module
    {
        #region implemented abstract members of NinjectModule

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StubImageDecoder>().As<ImageDecoder>();
            builder.RegisterType<SQLitePlatformWP8>().As<ISQLitePlatform>();
            builder.RegisterType<ShellToastNotificationService>().As<INotificationService>();
        }

        #endregion
    }
}