using System;
using NUnit.Framework;
using Spectator.Core.Tests.Common;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class AuthTests
	{
		TestModule injectModule;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		public void Test()
		{
			var module = new Account ();

			module.Login ("test-token").Wait ();
		}
	}
}