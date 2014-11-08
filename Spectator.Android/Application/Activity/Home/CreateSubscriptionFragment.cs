using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Spectator.Core.Model;

namespace Spectator.Android.Application.Activity.Home
{
	public class CreateSubscriptionFragment : DialogFragment
	{
		LinearLayout rssList;
		EditText title;
		EditText link;
		View rssButton;
		View okButton;
		View progress;

		IController[] controllers;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
			SetStyle (DialogFragmentStyle.NoTitle, 0);

			controllers = new IController[] {
				new ExtractRssController (this),
				new CreateSubscriptionController (this)
			};
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
			foreach (var s in controllers)
				s.OnActivityCreated ();
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var v = inflater.Inflate (Resource.Layout.fragment_create_subscription, null);
			link = v.FindViewById<EditText> (Resource.Id.link);
			title = v.FindViewById<EditText> (Resource.Id.title);
			rssButton = v.FindViewById (Resource.Id.rss);
			okButton = v.FindViewById (Resource.Id.ok);
			progress = v.FindViewById (Resource.Id.rssProgress);
			rssList = v.FindViewById<LinearLayout> (Resource.Id.rssList);
			return v;
		}

		interface IController
		{
			void OnActivityCreated ();
		}

		class CreateSubscriptionController : IController
		{
			CreateSubscriptionFragment fragment;

			public CreateSubscriptionController (CreateSubscriptionFragment fragment)
			{
				this.fragment = fragment;
			}

			public void OnActivityCreated ()
			{
				fragment.okButton.Click += OnClickedCreateSubscriptions;
			}

			void OnClickedCreateSubscriptions (object sender, EventArgs e)
			{
				if (ValidCreateData ())
					CreateSubscription ();
			}

			bool ValidCreateData ()
			{
				fragment.link.Error = Uri.IsWellFormedUriString (fragment.link.Text, UriKind.Absolute) 
					? null : "Not valid URL";
				fragment.title.Error = string.IsNullOrWhiteSpace (fragment.title.Text)
					? "Required field" : null;
				return fragment.link.Error == null && fragment.title.Error == null;
			}

			async void CreateSubscription ()
			{
				SetProgressEnabled (true);
				try {
					await new SubscriptionModel ().CreateNew (new Uri (fragment.link.Text), fragment.title.Text);
					fragment.DismissAllowingStateLoss ();
				} catch {
				}
				SetProgressEnabled (false);
			}

			void SetProgressEnabled (bool inProgress)
			{
				fragment.progress.Visibility = inProgress ? ViewStates.Visible : ViewStates.Gone;
				fragment.rssButton.Enabled = fragment.okButton.Enabled = !inProgress;
				fragment.rssList.Visibility = inProgress ? ViewStates.Gone : ViewStates.Visible;
			}
		}

		class ExtractRssController : IController
		{
			CreateSubscriptionFragment fragment;

			public ExtractRssController (CreateSubscriptionFragment fragment)
			{
				this.fragment = fragment;
			}

			public void OnActivityCreated ()
			{
				fragment.rssButton.Click += OnClickExtractRss;
			}

			void OnClickExtractRss (object sender, EventArgs e)
			{
				if (ValidRssData ())
					ExtractRss ();
			}

			bool ValidRssData ()
			{
				var result = Uri.IsWellFormedUriString (fragment.link.Text, UriKind.Absolute);
				fragment.link.Error = result ? null : "Not valid URL";
				return result;
			}

			async void ExtractRss ()
			{
				SetRssExportProgress (true);
				fragment.rssList.RemoveAllViews ();
				var extractor = new RssExtractor (new Uri (fragment.link.Text));
				var rssItems = await extractor.ExtracRss ();
				InitializeList (rssItems);
				SetRssExportProgress (false);
			}

			void SetRssExportProgress (bool inProgress)
			{
				fragment.progress.Visibility = inProgress ? ViewStates.Visible : ViewStates.Gone;
				fragment.rssButton.Enabled = fragment.okButton.Enabled = !inProgress;
			}

			void InitializeList (RssExtractor.RssItem[] rssItems)
			{
				foreach (var item in rssItems) {
					var view = CreateRssView (item);
					fragment.rssList.AddView (view);
				}
			}

			View CreateRssView (RssExtractor.RssItem item)
			{
				var button = new Button (fragment.Activity) { Text = item.Title };
				button.Click += (sender, e) => SelectRssItem (item);
				return button;
			}

			void SelectRssItem (RssExtractor.RssItem item)
			{
				fragment.title.Text = item.Title;
				fragment.link.Text = "" + item.Link;
			}
		}
	}
}