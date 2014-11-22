using Autofac;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using Spectator.Core.Model.Push;
using XamarinCommons.Image;

namespace Spectator.Android.Application.Model
{
	public class AndroidInjectModule : Module
	{
		#region implemented abstract members of NinjectModule

		protected override void Load (ContainerBuilder builder)
		{
			builder.RegisterType<BitmapImageDecoder> ().As<ImageDecoder> ();
			builder.RegisterType<SQLitePlatformAndroid>().As<ISQLitePlatform>();
			builder.RegisterType<NotificationService> ().As<INotificationService> ();
		}

		#endregion
	}
}