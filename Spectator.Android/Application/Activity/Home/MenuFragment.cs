using System;
using Bundle = global::Android.OS.Bundle;
using Spectator.Core.Model;
using Microsoft.Practices.ServiceLocation;
using Android.Widget;
using Android.Views;
using Android.Support.V4.Widget;
using Android.OS;
using Spectator.Android.Application.Activity.Common.Base;

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

			refresh.SetColorScheme(
				global::Android.Resource.Color.HoloBlueBright,
				global::Android.Resource.Color.HoloGreenLight,
				global::Android.Resource.Color.HoloOrangeLight,
				global::Android.Resource.Color.HoloRedLight);
			refresh.Refresh += (sender, e) => {
				new Handler().PostDelayed(() => refresh.Refreshing = false, 2000);
			};

			var d = await model.GetAllAsync ();
			if (d.Error == null) {
				// TODO
			}
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
				h.title.Text = "Position = " + position;
				return convertView;
			}

			public override int Count {
				get { return 100; } // TODO 
			}

			#endregion

			private class SubscriptionViewHolder : Java.Lang.Object{

				internal TextView title;

				public static SubscriptionViewHolder Get (ref View convertView, ViewGroup parent)
				{
					if (convertView == null) {
						convertView = View.Inflate (parent.Context, Resource.Layout.item_subscription, null);
						var h = new SubscriptionViewHolder ();
						h.title = convertView.FindViewById<TextView>(Resource.Id.title);
						convertView.Tag = h;
						return h;
					} 
					return (SubscriptionViewHolder) convertView.Tag;
				}
			}
		}
	}
}