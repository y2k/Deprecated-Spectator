using Autofac;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.WindowsPhone;
using Spectator.Core.Model.Push;
using XamarinCommons.Image;

namespace Spectator.WP8.ScheduledTaskAgent
{
    public class WPInjectModule : Module
    {
        #region implemented abstract members of NinjectModule

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StubImageDecoder>().As<ImageDecoder>();
            builder.RegisterType<MvxWindowsPhoneSQLiteConnectionFactory>().As<ISQLiteConnectionFactory>();
            builder.RegisterType<ShellToastNotificationService>().As<INotificationService>();
        }

        #endregion
    }
}