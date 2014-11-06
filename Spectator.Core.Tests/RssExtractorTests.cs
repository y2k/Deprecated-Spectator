using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Inject;
using Spectator.Core.Tests.Common;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class RssExtractorTests
	{
		TestModule injectModule;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		[Test]
		public async void TestJoyReactor ()
		{
			await Validate (
				new Uri ("http://joyreactor.cc/"), "JoyReactor.cc.html",
				new RssExtractor.RssItem {
					Title = string.Empty,
					Link = new Uri ("http://joyreactor.cc/rss")
				});
		}

		[Test]
		public async void TestHabrahabr ()
		{
			await Validate (
				new Uri ("http://habrahabr.ru/"), "habrahabr.ru.html",
				new RssExtractor.RssItem {
					Title = "Хабрахабр / Лучшие публикации за сутки",
					Link = new Uri ("http://habrahabr.ru/rss/best/")
				});
		}

		[Test]
		public async void TestYandeRe ()
		{
			await Validate (
				new Uri ("https://yande.re/post"), "yande.re.html",
				new RssExtractor.RssItem {
					Title = "RSS",
					Link = new Uri ("https://yande.re/post/piclens?page=1")
				},
				new RssExtractor.RssItem {
					Title = "ATOM",
					Link = new Uri ("https://yande.re/post/atom")
				});
		}

		async Task Validate (Uri source, string htmlPath, params RssExtractor.RssItem[] expected)
		{
			var module = new RssExtractor (new HttpClient (new MockHttpMessageHandler (htmlPath)), source);
			var rss = await module.ExtracRss ();
			CollectionAssert.AreEqual (rss, expected);
		}

		class MockHttpMessageHandler : HttpMessageHandler
		{
			string filename;

			public MockHttpMessageHandler (string filename)
			{
				this.filename = filename;
			}

			#region implemented abstract members of HttpMessageHandler

			protected override Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
			{
				return Task.Run<HttpResponseMessage> (() => {
					return new HttpResponseMessage (HttpStatusCode.OK) {
						Content = new StreamContent (File.OpenRead (Path.Combine ("Resources", filename)))
					};
				});
			}

			#endregion
		}
	}
}