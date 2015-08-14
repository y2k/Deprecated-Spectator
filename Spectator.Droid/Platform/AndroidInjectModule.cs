using Autofac;
using Spectator.Core.Model.Push;
using XamarinCommons.Image;

namespace Spectator.Droid.Platform
{
	public class AndroidInjectModule : Module
	{
		#region implemented abstract members of NinjectModule

		protected override void Load (ContainerBuilder builder)
		{
			builder.RegisterType<BitmapImageDecoder> ().As<ImageDecoder> ();
			builder.RegisterType<NotificationService> ().As<INotificationService> ();
		}

		#endregion
	}
}