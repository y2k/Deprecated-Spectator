using System;
using Bundle = global::Android.OS.Bundle;
using Spectator.Core.Model;
using Microsoft.Practices.ServiceLocation;
using Android.Widget;
using Android.Views;
using Android.Support.V4.Widget;
using Android.OS;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Core.Model.Database;
using System.Collections.Generic;
using Spectator.Android.Application.Widget;
using System.Text;

namespace Spectator.Android.Application.Activity.Home
{
	public class MenuFragment : BaseFragment
	{
		private ISubscriptionModel model = ServiceLocator.Current.GetInstance<ISubscriptionModel> ();

		private TextView error;
		private SwipeRefreshLayout refresh;
		private ListView list;

		public async override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			list.Adapter = new SubscriptionAdapter ();

			refresh.SetColorScheme (
				global::Android.Resource.Color.HoloBlueBright,
				global::Android.Resource.Color.HoloGreenLight,
				global::Android.Resource.Color.HoloOrangeLight,
				global::Android.Resource.Color.HoloRedLight);
			refresh.Refresh += (sender, e) => {
				new Handler ().PostDelayed (() => refresh.Refreshing = false, 2000);
			};

			var d = await model.GetAllAsync ();
			if (d.Error == null)
				((SubscriptionAdapter)list.Adapter).ChangeData (d.Value);
			error.Visibility = d.Error == null ? ViewStates.Gone : ViewStates.Visible;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.fragment_menu, null);
			error = view.FindViewById<TextView> (Resource.Id.error);
			refresh = view.FindViewById<SwipeRefreshLayout> (Resource.Id.refresh);
			list = view.FindViewById<ListView> (Resource.Id.list);
			return view;
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
				return position;
			}

			public override View GetView (int position, View convertView, ViewGroup parent)
			{
				var h = SubscriptionViewHolder.Get (ref convertView, parent);
				var i = items [position];
				h.title.Text = i.Title;
				h.image.ImageSource = GetThumbnailUrl (i.ThumbnailImageId, (int)(50 * parent.Resources.DisplayMetrics.Density));
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

				var url = new StringBuilder ("http://debug.spectator.api-i-twister.net/Image/Thumbnail/");
				url.Append (imageId);
				url.Append ("?width=" + maxWidthPx);
				url.Append ("&height=" + maxWidthPx);
				if (Build.VERSION.SdkInt >= Build.VERSION_CODES.JellyBean)
					url.Append ("&type=webp");
				return url.ToString ();
			}

			private class SubscriptionViewHolder : Java.Lang.Object
			{

				internal TextView title;
				internal WebImageView image;

				public static SubscriptionViewHolder Get (ref View convertView, ViewGroup parent)
				{
					if (convertView == null) {
						convertView = View.Inflate (parent.Context, Resource.Layout.item_subscription, null);
						convertView.Tag = new SubscriptionViewHolder {
							title = convertView.FindViewById<TextView> (Resource.Id.title),
							image = convertView.FindViewById<WebImageView> (Resource.Id.image),
						};
					} 
					return (SubscriptionViewHolder)convertView.Tag;
				}
			}
		}
	}
}