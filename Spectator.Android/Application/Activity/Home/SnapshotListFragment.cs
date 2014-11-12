using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Com.Android.EX.Widget;
using Spectator.Core;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Exceptions;
using Spectator.Android.Application.Activity.Common;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Android.Application.Activity.Common.Commands;
using Spectator.Android.Application.Activity.Profile;
using Spectator.Android.Application.Activity.Snapshots;
using Spectator.Android.Application.Widget;
using Bundle = global::Android.OS.Bundle;
using Color = global::Android.Graphics.Color;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.Android.Application.Activity.Home
{
	public class SnapshotListFragment : BaseFragment
	{
		StaggeredGridView list;
		SwipeRefreshLayout refresh;
		View errorGeneral;
		View errorAuth;

		SnapshotCollectionModel model;
		SelectSubscrptionCommand command;

		bool inProgress;
		IEnumerable<Snapshot> data;
		Exception error;

		#region Menu

		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
		{
			inflater.Inflate (Resource.Menu.snapshots, menu);
		}

		public  override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (item.ItemId == Resource.Id.delete)
				DeleteSubscription ();
			return true;
		}

		async void DeleteSubscription ()
		{
			try {
				await new SubscriptionModel ().Delete (model.SubscriptionId);
				ResetList (0);
			} catch {
				Toast.MakeText (Activity, Resource.String.error_cant_delete_subscription, ToastLength.Long).Show ();
			}
		}

		#endregion

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
			SetHasOptionsMenu (true);

			model = new SnapshotCollectionModel (0);
			ReloadData ();
		}

		bool IsViewCreated { get { return refresh != null; } }

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			command = new SelectSubscrptionCommand (ResetList);
			list.ColumnCount = 2;
			list.Adapter = new SnapshotAdapter ();

			refresh.Refresh += (sender, e) => ReloadData ();
			errorAuth.Click += (sender, e) => StartActivity (new Intent (Activity, typeof(ProfileActivity)));

			InvalidateUi ();
		}

		void ResetList (int newId)
		{
			model = new SnapshotCollectionModel (newId);
			ReloadData ();
		}

		async void ReloadData ()
		{
			inProgress = true;
			data = null;
			error = null;
			InvalidateUi ();

			await model.Reset ();
			try {
				await model.Next ();
				data = await model.Get ();
			} catch (Exception e) {
				error = e;
			}

			inProgress = false;
			InvalidateUi ();
		}

		void InvalidateUi ()
		{
			if (IsViewCreated) {
				refresh.Refreshing = inProgress;
				((SnapshotAdapter)list.Adapter).ChangeData (data);

				errorAuth.Visibility = errorGeneral.Visibility = ViewStates.Gone;
				if (error is NotAuthException)
					errorAuth.Visibility = ViewStates.Visible;
				else if (error != null)
					errorGeneral.Visibility = ViewStates.Visible;
			}
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			command.Close ();
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var v = inflater.Inflate (Resource.Layout.fragment_snapshots, null);
			refresh = v.FindViewById<SwipeRefreshLayout> (Resource.Id.refresh);
			list = v.FindViewById<StaggeredGridView> (Resource.Id.list);
			errorGeneral = v.FindViewById (Resource.Id.errorGeneral);
			errorAuth = v.FindViewById (Resource.Id.errorAuth);
			v.FindViewById (Resource.Id.createSubscription).Click += HandleClickCreateSubscription;
			return v;
		}

		void HandleClickCreateSubscription (object sender, EventArgs e)
		{
			new CreateSubscriptionFragment ().Show (FragmentManager, null);
		}

		class SnapshotAdapter : BaseAdapter
		{
			List<Snapshot> items = new List<Snapshot> ();
			PaletteController.Fabric paletteFabric = new PaletteController.Fabric ();

			static readonly Color DEFAULT_BACKGROUND = new Color (0x57, 0xC2, 0xAD);
			static readonly Color DEFAULT_FOREGROUND = new Color (0x1D, 0x63, 0x5A);

			ImageModel imageModel = new ImageModel ();

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

				h.textPanel.SetBackgroundColor (DEFAULT_BACKGROUND);
				h.title.SetTextColor (DEFAULT_FOREGROUND);
				if (h.justCreated) {
					var c = paletteFabric.NewInstance (h.image);
					c.AddView (h.textPanel, s => s.LightVibrantColor, (v, s) => v.SetBackgroundColor (new Color (s.Rgb)));
					c.AddView (h.title, s => s.LightVibrantColor, (v, s) => v.SetTextColor (PaletteController.InvertColor (new Color (s.Rgb))));
				}

				h.title.Text = i.Title;
				h.image.ImageSource = imageModel.GetThumbnailUrl (i.ThumbnailImageId, (int)(200 * parent.Resources.DisplayMetrics.Density));
				h.imagePanel.MaxSize = new Size (i.ThumbnailWidth, i.ThumbnailHeight);

				convertView.SetClick ((sender, e) => parent.Context.StartActivity (SnapshotActivity.NewIntent (i.Id)));

				return convertView;
			}

			public override int Count {
				get { return items.Count; }
			}

			#endregion

			class SnapshotViewHolder : Java.Lang.Object
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