using Autofac;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.WindowsPhone;
using Spectator.WP8.Model;
using Spectator.WP8.ViewModel;
using XamarinCommons.Image;

namespace Spectator.Android.Application.Model
{
    public class WPInjectModule : Module
    {
        #region implemented abstract members of NinjectModule

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StubImageDecoder>().As<ImageDecoder>();
            builder.RegisterType<MvxWindowsPhoneSQLiteConnectionFactory>().As<ISQLiteConnectionFactory>();

            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
        }

        #endregion
    }
}