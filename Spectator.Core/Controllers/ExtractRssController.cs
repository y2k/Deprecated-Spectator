using System;
using Spectator.Core.Model;
using System.Collections.Generic;

namespace Spectator.Core.Controllers
{
    [Obsolete]
    public class ExtractRssController
	{
		public string Link { get; set; }

		public bool LinkError { get; private set; }

		public bool InProgress { get; private set; }

		public Action UpdateUiCallback { get; set; }

		public List<RssItemController> RssItems { get; } = new List<RssItemController>();

		public void OnClickExtractRss ()
		{
			if (ValidRssData ())
				ExtractRss ();
		}

		bool ValidRssData ()
		{
			LinkError = !Uri.IsWellFormedUriString (Link, UriKind.Absolute);
			UpdateUi ();
			return !LinkError;
		}

		async void ExtractRss ()
		{
			SetRssExportProgress (true);
			RssItems.Clear ();
			UpdateUi ();

			var extractor = new RssExtractor (new Uri (Link));
			try {
				InitializeList (await extractor.ExtracRss ());
			} catch {
			}
			SetRssExportProgress (false);
		}

		void SetRssExportProgress (bool inProgress)
		{
			InProgress = inProgress;
			UpdateUi ();
		}

		void InitializeList (RssExtractor.RssItem[] rssItems)
		{
			foreach (var s in rssItems) {
				RssItems.Add (new RssItemController { Title = s.Title, Link = "" + s.Link });
			}
			UpdateUi ();
		}

		void UpdateUi ()
		{
			UpdateUiCallback ();
		}

		public class RssItemController
		{
			public string Title { get; set; }

			public string Link { get; set; }
		}
	}
}