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
using Spectator.Android.Application.Widget;
using System.Text;
using Spectator.Core.Model.Database;
using Spectator.Android.Application.Activity.Common.Commands;
using Com.Android.EX.Widget;

namespace Spectator.Android.Application.Activity.Home
{
	public class SnapshotFragment : BaseFragment
	{
		private StaggeredGridView list;
		private SwipeRefreshLayout refresh;
		private View errorGeneral;
		private View errorAuth;

		private ISnapshotCollectionModel model = ServiceLocator.Current.GetInstance<ISnapshotCollectionModel> ();

		private SelectSubscrptionCommand command;

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			command = new SelectSubscrptionCommand (id => LoadData (id));
			list.ColumnCount = 2;
			list.Adapter = new SnapshotAdapter ();

			refresh.SetColorScheme (
				global::Android.Resource.Color.HoloBlueBright,
				global::Android.Resource.Color.HoloGreenLight,
				global::Android.Resource.Color.HoloOrangeLight,
				global::Android.Resource.Color.HoloRedLight);
			refresh.Refresh += (sender, e) => LoadData (0);
			errorAuth.Click += (sender, e) => StartActivity (new Intent (Activity, typeof(ProfileActivity)));

			LoadData (0);
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			command.Close ();
		}

		private async void LoadData (long subId)
		{
			errorGeneral.Visibility = errorAuth.Visibility = ViewStates.Gone;
			refresh.Refreshing = true;
			((SnapshotAdapter)list.Adapter).ChangeData (null);

			try {
				((SnapshotAdapter)list.Adapter).ChangeData (await model.GetAllAsync (subId));
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
			list = v.FindViewById<StaggeredGridView> (Resource.Id.list);
			errorGeneral = v.FindViewById (Resource.Id.errorGeneral);
			errorAuth = v.FindViewById (Resource.Id.errorAuth);
			return v;
		}

		private class SnapshotAdapter : BaseAdapter
		{
			private List<Snapshot> items = new List<Snapshot> ();

			public void ChangeData (IEnumerable<Snapshot> items)
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
				var h = SnapshotViewHolder.Get (ref convertView, parent);
				h.title.Text = items [position].Title;
				h.image.ImageSource = GetThumbnailUrl (items [position].ThumbnailImageId, (int)(200 * parent.Resources.DisplayMetrics.Density));
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

			private class SnapshotViewHolder : Java.Lang.Object
			{
				public TextView title;
				public WebImageView image;

				public static SnapshotViewHolder Get (ref View convertView, ViewGroup parent)
				{
					if (convertView == null) {
						convertView = View.Inflate (parent.Context, Resource.Layout.item_snapshot, null);
						convertView.LayoutParameters = new StaggeredGridView.LayoutParams (StaggeredGridView.LayoutParams.WrapContent);

						convertView.Tag = new SnapshotViewHolder {
							title = convertView.FindViewById<TextView> (Resource.Id.title),
							image = convertView.FindViewById<WebImageView> (Resource.Id.image),
						};
					} 
					return (SnapshotViewHolder)convertView.Tag;
				}
			}
		}
	}
}