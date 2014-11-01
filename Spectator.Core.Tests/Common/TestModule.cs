using Autofac;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Wpf;
using Spectator.Core.Model.Web;
using System;
using System.Collections.Generic;
using Moq;

namespace Spectator.Core.Tests.Common
{
	public class TestModule : Module
	{
		Dictionary<Type, object> list = new Dictionary<Type, object> ();

		protected override void Load (ContainerBuilder builder)
		{
			builder.RegisterType<MvxWpfSqLiteConnectionFactory> ().As<ISQLiteConnectionFactory> ();
			builder.RegisterType<MockWebConnect> ().As<IWebConnect> ();

			foreach (var t in list.Keys)
				builder.RegisterInstance (list [t]).As (t);
		}

		public Mock<T> Set<T> (T instance) where T : class
		{
			list.Add (typeof(T), instance);
			return Mock.Get (instance);
		}
	}
}