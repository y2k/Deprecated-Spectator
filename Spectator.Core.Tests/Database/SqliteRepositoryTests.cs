using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Inject;
using Spectator.Core.Tests.Common;
using System.Collections.Generic;

namespace Spectator.Core.Tests.Database
{
	[TestFixture]
	public class SqliteRepositoryTests
	{
		TestModule injectModule;
		SqliteRepository module;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
			module = (SqliteRepository)ServiceLocator.Current.GetInstance<IRepository> ();
		}

		[Test]
		public void TestGetSubscriptions ()
		{
			module.ReplaceAll (new Subscription[0]);
			var actual = module.GetSubscriptions ();
			Assert.AreEqual (0, actual.Count ());

			module.ReplaceAll (GenerateSubscriptions (100));
			actual = module.GetSubscriptions ();
			Assert.AreEqual (100, actual.Count ());
			AssertIsOrdered (actual);
		}

		List<Subscription> GenerateSubscriptions (int count)
		{
			var rand = new Random (0);
			var result = new List<Subscription> ();
			for (int i = 0; i < count; i++)
				result.Add (new Subscription {
					Title = "Title " + rand.Next (),
					GroupTitle = "Group " + rand.Next (),
				});
			return result;
		}

		void AssertIsOrdered (IEnumerable<Subscription> actual)
		{
			CollectionAssert.AreEqual (
				actual.OrderBy (s => s.GroupTitle).ThenBy (s => s.Title),
				actual);
		}
	}
}