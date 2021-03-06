﻿using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using Spectator.Core.Tests.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spectator.Core.Tests
{
    [TestFixture]
	public class SubscriptionCollectionModelTests
	{
		TestModule injectModule;
		Mock<ISpectatorApi> api;
		Mock<IRepository> repo;
		SubscriptionCollectionModel module;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));

			api = injectModule.Set (Mock.Of<ISpectatorApi> ());
			repo = injectModule.Set (Mock.Of<IRepository> ());
			module = new SubscriptionCollectionModel ();
		}

		[Test]
		public async void GetListTest ()
		{
			repo.Setup (s => s.GetSubscriptions ()).Returns (new List<Subscription> ());
			var actual = await module.Get ();
			Assert.AreEqual (0, actual.Count ());
			repo.Verify (s => s.GetSubscriptions ());

			api.Setup (s => s.GetSubscriptions ()).Returns (Task.FromResult(GenerateApiSubscriptions (100)));
			await module.Reload ();
			api.Verify (s => s.GetSubscriptions (), Times.Once);
			repo.Verify (s => s.ReplaceAll (It.IsAny<IEnumerable<Subscription>> ()), Times.Once);

			repo.Setup (s => s.GetSubscriptions ()).Returns (GenerateRepoSubscriptions (100));
			actual = await module.Get ();
			Assert.AreEqual (100, actual.Count ());
			repo.Verify (s => s.GetSubscriptions ());
		}

		[Test]
		public async void TestNotAuthorized ()
		{
			api.Setup (s => s.GetSubscriptions ()).Throws<NotAuthException> ();

			try {
				await module.Reload ();
				Assert.Fail ();
			} catch (NotAuthException) {
			}
		}

		SubscriptionResponse GenerateApiSubscriptions (int count)
		{
			var result = new SubscriptionResponse ();
			for (int i = 0; i < count; i++)
				result.Subscriptions.Add (new SubscriptionResponse.ProtoSubscription ());
			return result;
		}

		List<Subscription> GenerateRepoSubscriptions (int count)
		{
			var result = new List<Subscription> ();
			for (int i = 0; i < count; i++)
				result.Add (new Subscription ());
			return result;
		}
	}
}