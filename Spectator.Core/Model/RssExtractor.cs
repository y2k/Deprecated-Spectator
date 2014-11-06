using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;

namespace Spectator.Core.Model
{
	public class RssExtractor
	{
		static string[] FeedTypes = new [] { "application/atom+xml", "application/rss+xml" };

		HttpClient client;
		Uri pageUri;

		public RssExtractor (HttpClient client, Uri pageUri)
		{
			this.client = client;
			this.pageUri = pageUri;
		}

		public Task<RssItem[]> ExtracRss ()
		{
			return Task.Run<RssItem[]> (() => {
				var doc = LoadDocument ();
				return doc.DocumentNode
					.Descendants ("link")
					.Where (s => IsRssOrAtomLink (s))
					.Select (s => NotToRssItem (s))
					.ToArray ();
			});
		}

		HtmlDocument LoadDocument ()
		{
			using (var stream = client.GetStreamAsync (pageUri).Result) {
				var doc = new HtmlDocument ();
				doc.Load (stream);
				return doc;
			}
		}

		bool IsRssOrAtomLink (HtmlNode s)
		{
			var type = s.Attributes ["type"];
			return type != null && FeedTypes.Contains (type.Value);
		}

		RssItem NotToRssItem (HtmlNode s)
		{
			return new RssItem {
				Title = s.Attributes ["title"].Value,
				Link = new Uri (pageUri, s.Attributes ["href"].Value)
			};
		}

		public class RssItem
		{
			public Uri Link { get; set; }

			public string Title { get; set; }
		}
	}
}

