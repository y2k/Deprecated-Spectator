using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Spectator.Droid.Activities.Common;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Bundle = global::Android.OS.Bundle;
using Spectator.Droid.Widgets;
using Spectator.Droid;

namespace Spectator.Droid.Activities.Home
{
	public class MenuFragment : BaseFragment
	{
		SubscriptionCollectionModel model = new SubscriptionCollectionModel ();

		TextView errorView;
		SwipeRefreshLayout refresh;
		ListView list;

		bool isProgress;
		IEnumerable<Subscription> items;
		Exception error;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			LoadSubscriptions ();
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			RetainInstance = true;
			base.OnActivityCreated (savedInstanceState);

			list.Adapter = new SubscriptionAdapter { Context = Activity };
			list.ItemClick += (sender, e) => new SelectSubscrptionCommand ((int)e.Id).Execute ();

			refresh.Refresh += (sender, e) => LoadSubscriptions ();
			InvalidateUI ();
		}

		async void LoadSubscriptions ()
		{
			isProgress = true;
			error = null;
			InvalidateUI ();
			try {
				await model.Reload ();
				items = await model.Get ();
			} catch (Exception e) {
				error = e;
			}
			isProgress = false;
			InvalidateUI ();
		}

		void InvalidateUI ()
		{
			if (IsViewCreated) {
				refresh.Refreshing = isProgress;
				((SubscriptionAdapter)list.Adapter).ChangeData (items);

				errorView.Visibility = error == null ? ViewStates.Gone : ViewStates.Visible;
				if (error is NotAuthException)
					errorView.SetText (Resource.String.auth_error);
				if (error != null)
					errorView.SetText (Resource.String.common_error);
			}
		}

		bool IsViewCreated { get { return refresh != null; } }

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.fragment_menu, null);
			errorView = view.FindViewById<TextView> (Resource.Id.error);
			refresh = view.FindViewById<SwipeRefreshLayout> (Resource.Id.refresh);
			list = view.FindViewById<ListView> (Resource.Id.list);
			return view;
		}

		class SubscriptionAdapter : BaseAdapter
		{
			public Context Context { get; set; }

			List<Subscription> items = new List<Subscription> ();

			public void ChangeData (IEnumerable<Subscription> items)
			{
				this.items.Clear ();
				if (items != null) {
					this.items.Add (CreateHeader ());
					this.items.AddRange (items);
				}
				NotifyDataSetChanged ();
			}

			Subscription CreateHeader ()
			{
				return new Subscription {
					Title = Context.GetString (Resource.String.feed),
				};
			}

			#region implemented abstract members of BaseAdapter

			public override Java.Lang.Object GetItem (int position)
			{
				return null;
			}

			public override long GetItemId (int position)
			{
				return items [position].Id;
			}

			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				var h = SubscriptionViewHolder.Get (ref convertView, parent);
				var i = items [position];
				h.title.Text = i.Title;
				h.count.Text = "" + i.UnreadCount;
				h.count.Visibility = i.UnreadCount > 0 ? ViewStates.Visible : ViewStates.Gone;
				h.image.ImageSource = GetThumbnailUrl (i.ThumbnailImageId, (int)(50 * parent.Resources.DisplayMetrics.Density));
				h.groupTitle.Text = i.GroupTitle;
				h.groupTitle.Visibility = CanShowGroupTitle (position) ? ViewStates.Visible : ViewStates.Gone;
				return convertView;
			}

			bool CanShowGroupTitle (int position)
			{
				return position > 0 && items [position - 1].GroupTitle != items [position].GroupTitle;
			}

			public override int Count {
				get { return items.Count; }
			}

			#endregion

			string GetThumbnailUrl (int imageId, int maxWidthPx)
			{
				if (imageId <= 0)
					return null;
                return new ImageIdToUrlConverter().GetThumbnailUrl(imageId, maxWidthPx);
			}

			class SubscriptionViewHolder : Java.Lang.Object
			{
				internal TextView title;
				internal WebImageView image;
				internal TextView count;
				internal TextView groupTitle;

				public static SubscriptionViewHolder Get (ref View convertView, ViewGroup parent)
				{
					if (convertView == null) {
						convertView = View.Inflate (parent.Context, Resource.Layout.item_subscription, null);
						convertView.Tag = new SubscriptionViewHolder {
							title = convertView.FindViewById<TextView> (Resource.Id.title),
							image = convertView.FindViewById<WebImageView> (Resource.Id.image),
							count = convertView.FindViewById<TextView> (Resource.Id.count),
							groupTitle = convertView.FindViewById<TextView> (Resource.Id.groupTitle),
						};
					} 
					return (SubscriptionViewHolder)convertView.Tag;
				}
			}
		}
	}
}