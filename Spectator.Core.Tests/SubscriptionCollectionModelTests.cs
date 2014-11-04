﻿using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model.Web;
using Spectator.Core.Tests.Common;
using System.Collections.Generic;
using Spectator.Core.Model.Web.Proto;
using Spectator.Core.Model.Exceptions;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class SubscriptionCollectionModelTests
	{
		TestModule injectModule;
		Mock<IApiClient> api;
		Mock<IRepository> repo;
		SubscrptionCollectionModel module;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));

			api = injectModule.Set (Mock.Of<IApiClient> ());
			repo = injectModule.Set (Mock.Of<IRepository> ());
			module = new SubscrptionCollectionModel ();
		}

		[Test]
		public void GetListTest ()
		{
			var actual = module.Get ().Result;
			Assert.AreEqual (0, actual.Count ());

			api.Setup (s => s.GetSubscriptions ()).Returns (GenerateApiSubscriptions (100));
			module.Reload ().Wait ();
			api.Verify (s => s.GetSubscriptions (), Times.Once);
			repo.Verify (s => s.ReplaceAll (It.IsAny<IEnumerable<Subscription>> ()), Times.Once);

			repo.Setup (s => s.GetSubscriptions ()).Returns (GenerateRepoSubscriptions (100));
			actual = module.Get ().Result;
			Assert.AreEqual (100, actual.Count ());
		}

		[Test (Description = "Test description")]
		public void TestNotAuthorized ()
		{
			api.Setup (s => s.GetSubscriptions ()).Throws<NotAuthException> ();

			try {
				module.Reload ().Wait ();
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