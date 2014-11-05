using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using Spectator.Core.Model.Account;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model.Web;
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
		public async void TestLoginAndLogout ()
		{
			var web = injectModule.Set<IApiClient> ();
			var module = new Account ();

			var testUserState = new Dictionary<string, string> { { "a",	"b"	} };
			web.Setup (s => s.LoginByCode (It.IsAny<string> ())).Returns (testUserState);
			await module.LoginByCode ("test-token");
			web.Verify (s => s.LoginByCode ("test-token"), Times.Once);

			var actual = new RepositoryAuthProvider ().Load ();
			Assert.AreEqual (testUserState, actual);

			await module.Logout ();
			actual = new RepositoryAuthProvider ().Load ();
			Assert.AreEqual (0, actual.Count);
		}
	}
}