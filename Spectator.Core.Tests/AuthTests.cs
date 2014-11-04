using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spectator.Core.Model.Account;
using Spectator.Core.Model.Inject;
using Spectator.Core.Tests.Common;

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

		[Test]
		public void TestLogin ()
		{
			var module = new Account ();
			module.LoginByCode ("test-token").Wait ();
		}

		[Test]
		public void TestLogout ()
		{
			var module = new Account ();
			module.Logout ().Wait ();
		}
	}
}