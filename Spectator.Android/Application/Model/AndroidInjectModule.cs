using System;
using Autofac;
using Spectator.Core.Model.Image;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Droid;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace Spectator.Android.Application.Model
{
	public class AndroidInjectModule : Module
	{
		#region implemented abstract members of NinjectModule

		protected override void Load (ContainerBuilder builder)
		{
			builder.RegisterType<BitmapImageDecoder> ().As<IImageDecoder> ();
			builder.RegisterType<MvxDroidSQLiteConnectionFactory> ().As<ISQLiteConnectionFactory>();
		}

		#endregion
	}
}