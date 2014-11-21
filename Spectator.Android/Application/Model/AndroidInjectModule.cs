using Autofac;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Droid;
using XamarinCommons.Image;
using Spectator.Core.Model.Push;

namespace Spectator.Android.Application.Model
{
	public class AndroidInjectModule : Module
	{
		#region implemented abstract members of NinjectModule

		protected override void Load (ContainerBuilder builder)
		{
			builder.RegisterType<BitmapImageDecoder> ().As<ImageDecoder> ();
			builder.RegisterType<MvxDroidSQLiteConnectionFactory> ().As<ISQLiteConnectionFactory> ();
			builder.RegisterType<NotificationService> ().As<INotificationService> ();
		}

		#endregion
	}
}