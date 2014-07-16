using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Android.EX.Widget;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Exceptions;
using global::Android.Support.V4.Widget;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Android.Application.Activity.Common.Commands;
using Spectator.Android.Application.Activity.Profile;
using Spectator.Android.Application.Widget;
using Bundle = global::Android.OS.Bundle;
using System.Drawing;
using Android.Support.V7.Graphics;
using Color = global::Android.Graphics.Color;
using System.Threading.Tasks;
using Android.Graphics;

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
		private long subscriptionId;

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			command = new SelectSubscrptionCommand (id => LoadData (id));
			list.ColumnCount = 2;
			list.Adapter = new SnapshotAdapter ();

			refresh.Refresh += (sender, e) => LoadData (subscriptionId);
			errorAuth.Click += (sender, e) => StartActivity (new Intent (Activity, typeof(ProfileActivity)));
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			command.Close ();
		}

		public override void OnStart ()
		{
			base.OnStart ();
			model.SnapshotChanged += HandleSnapshotChanged;
			LoadData (subscriptionId);
		}

		void HandleSnapshotChanged (object sender, SnapshotChangedArgs e)
		{
			// TODO
			if (e.SubscriptionId != subscriptionId)
				return;

			if (e.Error == null)
				((SnapshotAdapter)list.Adapter).ChangeData (e.Items);
			else if (e.Error is WrongAuthException)
				errorAuth.Visibility = ViewStates.Visible;
			else
				errorGeneral.Visibility = ViewStates.Visible;

			if (!e.FromCache)
				refresh.Refreshing = false;
		}

		public override void OnStop ()
		{
			base.OnStop ();
			model.SnapshotChanged -= HandleSnapshotChanged;
		}

		private void LoadData (long subId)
		{
			if (subscriptionId != subId)
				((SnapshotAdapter)list.Adapter).ChangeData (null);

			errorGeneral.Visibility = errorAuth.Visibility = ViewStates.Gone;
			refresh.Refreshing = true;

			subscriptionId = subId;
			model.RequestSnapshots (subscriptionId);

//			errorGeneral.Visibility = errorAuth.Visibility = ViewStates.Gone;
//			refresh.Refreshing = true;
//			((SnapshotAdapter)list.Adapter).ChangeData (null);
//
//			try {
//				((SnapshotAdapter)list.Adapter).ChangeData (await model.GetAllAsync (subId));
//			} catch (WrongAuthException) {
//				errorAuth.Visibility = ViewStates.Visible;
//			} catch (Exception) {
//				errorGeneral.Visibility = ViewStates.Visible;
//			} finally {
//				refresh.Refreshing = false;
//			}
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
			private PaletteController.Fabric paletteFabric = new PaletteController.Fabric();

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
				var i = items [position];

				h.textPanel.SetBackgroundColor (Color.LightGray);
				h.title.SetTextColor (Color.DarkGray);
				if (h.justCreated) {
					var c = paletteFabric.NewInstance(h.image);
					c.AddView (h.textPanel, s => s.LightVibrantColor, (v, s) => v.SetBackgroundColor (new Color (s.Rgb)));
					c.AddView (h.title, s => s.DarkMutedColor, (v, s) => v.SetTextColor (new Color (s.Rgb)));
				}

				h.title.Text = i.Title;
				h.image.ImageSource = GetThumbnailUrl (i.ThumbnailImageId, (int)(200 * parent.Resources.DisplayMetrics.Density));
				h.imagePanel.MaxSize = new Size (i.ThumbnailWidth, i.ThumbnailHeight);

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
				if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr2) url.Append ("&type=webp");
				return url.ToString ();
			}

			private class SnapshotViewHolder : Java.Lang.Object
			{
				public TextView title;
				public WebImageView image;
				public FixAspectFrameLayout imagePanel;
				public View textPanel;
				public bool justCreated;

				public static SnapshotViewHolder Get (ref View convertView, ViewGroup parent)
				{
					if (convertView == null) {
						convertView = View.Inflate (parent.Context, Resource.Layout.item_snapshot, null);
						convertView.LayoutParameters = new StaggeredGridView.LayoutParams (StaggeredGridView.LayoutParams.WrapContent);

						convertView.Tag = new SnapshotViewHolder {
							title = convertView.FindViewById<TextView> (Resource.Id.title),
							image = convertView.FindViewById<WebImageView> (Resource.Id.image),
							imagePanel = convertView.FindViewById<FixAspectFrameLayout> (Resource.Id.imagePanel),
							textPanel = convertView.FindViewById<View> (Resource.Id.textPanel),
							justCreated = true,
						};
					} else {
						((SnapshotViewHolder)convertView.Tag).justCreated = false;
					}
					return (SnapshotViewHolder)convertView.Tag;
				}
			}
		}
	}
}