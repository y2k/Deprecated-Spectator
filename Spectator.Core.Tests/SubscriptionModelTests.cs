using System;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model.Web;
using Spectator.Core.Tests.Common;
using Spectator.Core.Model.Database;
using System.Collections.Generic;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class SubscriptionModelTests
	{
		Mock<ISpectatorApi> api;
		Mock<IRepository> repo;
		SubscriptionModel module;
		TestModule injectModule;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));

			api = injectModule.Set<ISpectatorApi> ();
			repo = injectModule.Set<IRepository> ();
			module = new SubscriptionModel ();
		}

		[Test]
		public async void TestCreateNew ()
		{
			await module.CreateNew (new Uri ("http://google.com"), "Test subscription");
			api.Verify (s => s.CreateSubscription (new Uri ("http://google.com"), "Test subscription"), Times.Once);
		}

		[Test]
		public async void TestDelete ()
		{
			repo.Setup (s => s.GetSubscriptions ()).Returns (
				new List<Subscription> { new Subscription{ ServerId = 1000, Id = 1 } });
			await module.Delete (1);
			api.Verify (s => s.DeleteSubscription (1000), Times.Once);
		}

		[Test]
		public async void TestEdit ()
		{
			repo.Setup (s => s.GetSubscriptions ()).Returns (
				new List<Subscription> { new Subscription{ ServerId = 1000, Id = 1 } });
			await module.Edit (1, "New title");
			api.Verify (s => s.EditSubscription (1000, "New title"), Times.Once);
		}
	}
}