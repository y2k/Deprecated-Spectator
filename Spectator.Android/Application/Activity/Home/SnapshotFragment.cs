using System;
using Bundle = global::Android.OS.Bundle;
using Spectator.Android.Application.Activity.Common.Base;
using Android.Views;
using Android.Widget;
using Spectator.Core.Model;
using global::Android.Support.V4.Widget;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Exceptions;
using Spectator.Android.Application.Activity.Profile;
using Android.Content;
using Android.OS;
using System.Collections.Generic;

namespace Spectator.Android.Application.Activity.Home
{
	public class SnapshotFragment : BaseFragment
	{
		private ListView list;
		private SwipeRefreshLayout refresh;
		private View errorGeneral;
		private View errorAuth;

		private SnapshotAdapter adapter;

		private ISnapshotCollectionModel model = ServiceLocator.Current.GetInstance<ISnapshotCollectionModel>();

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			list.Adapter = adapter = new SnapshotAdapter ();

			refresh.SetColorScheme(
				global::Android.Resource.Color.HoloBlueBright,
				global::Android.Resource.Color.HoloGreenLight,
				global::Android.Resource.Color.HoloOrangeLight,
				global::Android.Resource.Color.HoloRedLight);
			refresh.Refresh += (sender, e) => LoadData();
			errorAuth.Click += (sender, e) => StartActivity (new Intent (Activity, typeof(ProfileActivity)));

			LoadData ();
		}

		private async void LoadData() {
			errorGeneral.Visibility = errorAuth.Visibility = ViewStates.Gone;
			refresh.Refreshing = true;
			try {
				adapter.ChangeData(await model.GetAllAsync(0));
			} catch (WrongAuthException) {
				errorAuth.Visibility = ViewStates.Visible;
			} catch (Exception) {
				errorGeneral.Visibility = ViewStates.Visible;
			} finally {
				refresh.Refreshing = false;
			}
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var v = inflater.Inflate (Resource.Layout.fragment_snapshots, null);
			refresh = v.FindViewById<SwipeRefreshLayout> (Resource.Id.refresh);
			list = v.FindViewById<ListView> (Resource.Id.list);
			errorGeneral = v.FindViewById (Resource.Id.errorGeneral);
			errorAuth = v.FindViewById (Resource.Id.errorAuth);
			return v;
		}

		private class SnapshotAdapter : BaseAdapter 
		{
			private List<object> items = new List<object> ();

			public void ChangeData(IEnumerable<object> items) {
				this.items.Clear ();
				if (items != null) this.items.AddRange (items);
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
				var h = SnapshotViewHolder.Get (ref convertView, parent);
				h.title.Text = "Position = " + position;
				return convertView;
			}

			public override int Count {
				get { return items.Count; } // TODO 
			}

			#endregion

			private class SnapshotViewHolder : Java.Lang.Object{

				internal TextView title;

				public static SnapshotViewHolder Get (ref View convertView, ViewGroup parent)
				{
					if (convertView == null) {
						convertView = View.Inflate (parent.Context, Resource.Layout.item_snapshot, null);
						var h = new SnapshotViewHolder ();
						h.title = convertView.FindViewById<TextView>(Resource.Id.title);
						convertView.Tag = h;
						return h;
					} 
					return (SnapshotViewHolder) convertView.Tag;
				}
			}
		}
	}
}