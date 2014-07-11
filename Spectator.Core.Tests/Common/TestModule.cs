using NUnit.Framework;
using System;
using Autofac;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Wpf;
using Spectator.Core.Model.Web;

namespace Spectator.Core.Tests.Common
{
	public class TestModule : Module
	{
		protected override void Load (ContainerBuilder builder)
		{
			builder.RegisterType<MvxWpfSqLiteConnectionFactory> ().As<ISQLiteConnectionFactory>();
			builder.RegisterType<MockWebConnect> ().As<IWebConnect> ();
		}
	}
}