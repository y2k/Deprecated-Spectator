using Autofac;
using XamarinCommons.Image;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Touch;

namespace Spectator.Ios.Model
{
	public class PlatformInjectModule : Module
	{
		protected override void Load (ContainerBuilder builder)
		{
			builder.RegisterType<UIImageDecoder> ().As<ImageDecoder> ();
			builder.RegisterType<MvxTouchSQLiteConnectionFactory> ().As<ISQLiteConnectionFactory>();
		}
	}
}