using System;
using Autofac;
using Spectator.Core.Model.Image;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.WindowsPhone;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Spectator.WP8.ViewModel;

namespace Spectator.Android.Application.Model
{
	public class WPInjectModule : Module
	{
		#region implemented abstract members of NinjectModule

		protected override void Load (ContainerBuilder builder)
		{
            //builder.RegisterType<BitmapImageDecoder> ().As<IImageDecoder> ();
			builder.RegisterType<MvxWindowsPhoneSQLiteConnectionFactory> ().As<ISQLiteConnectionFactory>();

            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
		}

		#endregion
	}
}