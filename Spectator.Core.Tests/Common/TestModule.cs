using System;
using System.Collections.Generic;
using Autofac;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Cirrious.MvvmCross.Community.Plugins.Sqlite.Wpf;
using Moq;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Tests.Common
{
	public class TestModule : Module
	{
		Dictionary<Type, object> list = new Dictionary<Type, object> ();
		IRepository repo;

		public TestModule ()
		{
			repo = CreateMemoryRepository ();
		}

		protected override void Load (ContainerBuilder builder)
		{
			builder.RegisterType<MvxWpfSqLiteConnectionFactory> ().As<ISQLiteConnectionFactory> ();
			builder.RegisterInstance (repo).As<IRepository> ();

			foreach (var t in list.Keys)
				builder.RegisterInstance (list [t]).As (t);
		}

		IRepository CreateMemoryRepository ()
		{
			var db = new MvxWpfSqLiteConnectionFactory ().CreateInMemory ();
			ConnectionOpenHelper.CreateTabled (db);
			return new SqliteRepository (db);
		}

		public Mock<T> Set<T> (T instance) where T : class
		{
			list.Add (typeof(T), instance);
			return Mock.Get (instance);
		}

		public Mock<T> Set<T> () where T : class
		{
			return Set (Mock.Of<T> ());
		}
	}
}