using NUnit.Framework;
using Spectator.Core.Tests.Common;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Inject;
using Moq;
using Spectator.Core.Model.Web;

namespace Spectator.Core.Tests.Web
{
	[TestFixture]
	public class HttpApiClientTests
	{
		TestModule injectModule;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		[Test]
		public void Test ()
		{
			var cookies = injectModule.Set (Mock.Of<IAuthStorage> ());

			var module = new HttpApiClient ();

			Assert.Fail ("Not implemented");
		}
	}
}