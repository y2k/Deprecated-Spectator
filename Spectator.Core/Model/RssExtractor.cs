using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Spectator.Core.Model
{
	public class RssExtractor
	{
		static string[] FeedTypes = { "application/atom+xml", "application/rss+xml" };

		HttpClient client;
		Uri pageUri;

		public RssExtractor (Uri pageUri) : this (new HttpClient (), pageUri)
		{
		}

		public RssExtractor (HttpClient client, Uri pageUri)
		{
			this.client = client;
			this.pageUri = pageUri;
		}

		public Task<RssItem[]> ExtracRss ()
		{
			return Task.Run(() => {
				var doc = LoadDocument ();
				return doc.DocumentNode
					.Descendants ("link")
					.Where (IsRssOrAtomLink)
					.Select (s => NodeToRssItem (s))
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

		RssItem NodeToRssItem (HtmlNode s)
		{
            return new RssItem {
				Title = s.Attributes ["title"].Value,
				Link = new Uri (pageUri, HtmlEntity.DeEntitize(s.Attributes ["href"].Value))
			};
		}

		public class RssItem
		{
			public Uri Link { get; set; }

			public string Title { get; set; }

			public override string ToString ()
			{
				return string.Format ("[RssItem: Link={0}, Title={1}]", Link, Title);
			}

			public override bool Equals (object obj)
			{
				return ToString ().Equals ("" + obj);
			}

			public override int GetHashCode ()
			{
				return ToString ().GetHashCode ();
			}
		}
	}
}