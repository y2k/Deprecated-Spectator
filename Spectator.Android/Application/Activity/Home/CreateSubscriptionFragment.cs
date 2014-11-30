using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Spectator.Core.Controllers;

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

		CreateSubscriptionController createController = new CreateSubscriptionController ();
		ExtractRssController extractController = new ExtractRssController ();

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
			SetStyle (StyleNoTitle, 0);
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			okButton.Click += (sender, e) => createController.OnClickedCreateSubscriptions ();
			link.TextChanged += (sender, e) => createController.Link = link.Text;
			title.TextChanged += (sender, e) => createController.Title = title.Text;
			createController.CallbackFinishSuccess = DismissAllowingStateLoss;
			createController.CallbackUpdateUi = UpdateUiForCreateSubscriptionController;

			link.TextChanged += (sender, e) => extractController.Link = link.Text;
			rssButton.Click += (sender, e) => extractController.OnClickExtractRss ();
			extractController.UpdateUiCallback = UpdateUiForExtractController;
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

		void UpdateUiForCreateSubscriptionController ()
		{
			var inProgress = createController.InProgress;
			progress.Visibility = inProgress ? ViewStates.Visible : ViewStates.Gone;
			rssButton.Enabled = okButton.Enabled = !inProgress;
			rssList.Visibility = inProgress ? ViewStates.Gone : ViewStates.Visible;

			title.Error = createController.TitleError ? GetString (Resource.String.required_field) : null;
			link.Error = createController.LinkError ? GetString (Resource.String.not_valid_url) : null;
		}

		void UpdateUiForExtractController ()
		{
			progress.Visibility = extractController.InProgress ? ViewStates.Visible : ViewStates.Gone;
			rssButton.Enabled = okButton.Enabled = !extractController.InProgress;
			link.Error = extractController.LinkError ? GetString (Resource.String.not_valid_url) : null;

			rssList.RemoveAllViews ();
			foreach (var s in extractController.RssItems)
				rssList.AddView (CreateRssView (s));
		}

		View CreateRssView (ExtractRssController.RssItemController item)
		{
			var button = new Button (Activity) { Text = item.Title };
			button.Click += (sender, e) => {
				title.Text = item.Title;
				link.Text = item.Link;
			};
			return button;
		}
	}
}