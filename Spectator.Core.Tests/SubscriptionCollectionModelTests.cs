using System;
using NUnit.Framework;
using Spectator.Core.Tests.Common;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model;
using System.Linq;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class SubscriptionCollectionModelTests
	{
		TestModule injectModule;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		[Test]
		public void GetListTest ()
		{
			var module = new SubscrptionCollectionModel ();

			var actual = module.Get ().Result;
			Assert.AreEqual (0, actual.Count ());

			module.Reload ().Wait ();
			actual = module.Get ().Result;
			Assert.AreEqual (100, actual.Count ());
		}
	}
}