using System;
using System.Collections.Generic;
using System.Text;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Android.Application.Activity.Common.Commands;
using Spectator.Android.Application.Widget;
using Bundle = global::Android.OS.Bundle;
using Android.Graphics;

namespace Spectator.Android.Application.Activity.Home
{
	public class MenuFragment : BaseFragment
	{
		private ISubscriptionModel model = ServiceLocator.Current.GetInstance<ISubscriptionModel> ();

		private TextView error;
		private SwipeRefreshLayout refresh;
		private ListView list;
		private bool loading;

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			list.Adapter = new SubscriptionAdapter ();
			list.ItemClick += (sender, e) => new SelectSubscrptionCommand (e.Id).Execute ();

			refresh.Refresh += (sender, e) => ReloadList (true);

			ReloadList (savedInstanceState == null);
			AddStartStopEvent ((s, e) => ReloadList (false), s => model.SubscriptionsChagned += s, s => model.SubscriptionsChagned -= s);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.fragment_menu, null);
			error = view.FindViewById<TextView> (Resource.Id.error);
			refresh = view.FindViewById<SwipeRefreshLayout> (Resource.Id.refresh);
			list = view.FindViewById<ListView> (Resource.Id.list);
			return view;
		}

		private async void ReloadList (bool fromWeb)
		{
			if (loading) return;
			loading = true;

			if (fromWeb) refresh.Refreshing = true;

			((SubscriptionAdapter)list.Adapter).ChangeData ((await model.GetAllAsync (false)).Value);

			if (fromWeb) {
				var d = await model.GetAllAsync (true);
				if (d.Error == null) ((SubscriptionAdapter)list.Adapter).ChangeData (d.Value);
				error.Visibility = d.Error == null ? ViewStates.Gone : ViewStates.Visible;
				refresh.Refreshing = false;
			}

			loading = false;
		}

		private class SubscriptionAdapter : BaseAdapter
		{
			private List<Subscription> items = new List<Subscription> ();

			public void ChangeData (IEnumerable<Subscription> items)
			{
				this.items.Clear ();
				if (items != null)
					this.items.AddRange (items);
				NotifyDataSetChanged ();
			}

			#region implemented abstract members of BaseAdapter

			public override Java.Lang.Object GetItem (int position)
			{
				return null;
			}

			public override long GetItemId (int position)
			{
				return items [position].ServerId;
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
				h.groupTitle.Visibility = (position == 0 || items [position - 1].GroupTitle != i.GroupTitle) ? ViewStates.Visible : ViewStates.Gone;
				return convertView;
			}

			public override int Count {
				get { return items.Count; }
			}

			#endregion

			private string GetThumbnailUrl (int imageId, int maxWidthPx)
			{
				if (imageId <= 0)
					return null;

				var url = new StringBuilder (Constants.BaseApi + "Image/Thumbnail/");
				url.Append (imageId);
				url.Append ("?width=" + maxWidthPx);
				url.Append ("&height=" + maxWidthPx);
				if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
					url.Append ("&type=webp");
				return url.ToString ();
			}

			private class SubscriptionViewHolder : Java.Lang.Object
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