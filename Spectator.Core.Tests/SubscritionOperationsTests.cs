using System;
using NUnit.Framework;
using Spectator.Core.Tests.Common;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model.Web;
using Spectator.Core.Model;
using Moq;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class SubscritionOperationsTests
	{
		TestModule injectModule;
		Mock<IApiClient> api;
		SubscriptionOperations module;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));

			api = injectModule.Set<IApiClient> ();
			module = new SubscriptionOperations ();
		}

		[Test]
		public async void TestCreateNew ()
		{
			await module.CreateNew (new Uri ("http://google.com"), "Test subscription");
		}

		[Test]
		public async void TestDelete ()
		{
			await module.Delete (0);
		}

		[Test]
		public async void TestEdit ()
		{
			await module.Edit (0, "New title");
		}
	}
}