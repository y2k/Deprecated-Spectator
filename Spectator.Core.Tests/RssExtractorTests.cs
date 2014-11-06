using System;
using NUnit.Framework;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Tests.Common;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class RssExtractorTests
	{
		static readonly Uri TestDocUrl = new Uri ("http://joyreactor.cc/");
		TestModule injectModule;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		[Test]
		public async void Test ()
		{
			var module = new RssExtractor (new HttpClient (new MockHttpMessageHandler ()), TestDocUrl);
			var rss = await module.ExtracRss ();

			Assert.AreEqual (1, rss.Length);
			Assert.AreEqual ("", rss [0].Title);
			Assert.AreEqual (new Uri ("http://joyreactor.cc/rss"), rss [0].Link);
		}

		private class MockHttpMessageHandler : HttpMessageHandler
		{
			#region implemented abstract members of HttpMessageHandler

			protected override Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
			{
				return Task.Run<HttpResponseMessage> (() => {
					return new HttpResponseMessage (HttpStatusCode.OK) {
						Content = new StreamContent (File.OpenRead ("Resources/JoyReactor.cc.html"))
					};
				});
			}

			#endregion
		}
	}
}