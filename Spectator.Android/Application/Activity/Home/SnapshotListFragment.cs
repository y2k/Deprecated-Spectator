using System;
using System.Collections.Generic;
using System.Drawing;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Exceptions;
using Spectator.Android.Application.Activity.Common;
using Spectator.Android.Application.Activity.Common.Base;
using Spectator.Android.Application.Activity.Common.Commands;
using Spectator.Android.Application.Activity.Snapshots;
using Spectator.Android.Application.Widget;
using Bundle = global::Android.OS.Bundle;
using Color = global::Android.Graphics.Color;
using Size = System.Drawing.Size;

namespace Spectator.Android.Application.Activity.Home
{
	public class SnapshotListFragment : BaseFragment
	{
		RecyclerView list;
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
			HasOptionsMenu = true;

			model = new SnapshotCollectionModel (0);
			ReloadData ();
		}

		bool IsViewCreated { get { return refresh != null; } }

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);

			command = new SelectSubscrptionCommand (ResetList);
			list.SetAdapter (new SnapshotAdapter (Activity));

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
				((SnapshotAdapter)list.GetAdapter ()).ChangeData (data);

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

			list = v.FindViewById<RecyclerView> (Resource.Id.list);
			list.SetLayoutManager (new StaggeredGridLayoutManager (2, StaggeredGridLayoutManager.Vertical));
			list.AddItemDecoration (new DividerItemDecoration (2));

			errorGeneral = v.FindViewById (Resource.Id.errorGeneral);
			errorAuth = v.FindViewById (Resource.Id.errorAuth);
			v.FindViewById (Resource.Id.createSubscription).Click += HandleClickCreateSubscription;
			return v;
		}

		void HandleClickCreateSubscription (object sender, EventArgs e)
		{
			new CreateSubscriptionFragment ().Show (FragmentManager, null);
		}

		class SnapshotAdapter : RecyclerView.Adapter
		{
			List<Snapshot> items = new List<Snapshot> ();
			PaletteController.Fabric paletteFabric = new PaletteController.Fabric ();

			static readonly Color DEFAULT_BACKGROUND = new Color (0x57, 0xC2, 0xAD);
			static readonly Color DEFAULT_FOREGROUND = new Color (0x1D, 0x63, 0x5A);

			ImageModel imageModel = ImageModel.Instance;

			Context context;

			public SnapshotAdapter (Context context)
			{
				this.context = context;
			}

			public void ChangeData (IEnumerable<Snapshot> items)
			{
				this.items.Clear ();
				if (items != null)
					this.items.AddRange (items);
				NotifyDataSetChanged ();
			}

			#region implemented abstract members of Adapter

			public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
			{
				var h = (SnapshotViewHolder)holder;
				var i = items [position];

				h.TextPanel.SetBackgroundColor (DEFAULT_BACKGROUND);
				h.Title.SetTextColor (DEFAULT_FOREGROUND);
				if (h.JustCreated) {
					var c = paletteFabric.NewInstance (h.Image);
					c.AddView (h.TextPanel, s => s.LightVibrantSwatch, (v, s) => v.SetBackgroundColor (new Color (s.Rgb)));
					c.AddView (h.Title, s => s.LightVibrantSwatch, (v, s) => v.SetTextColor (PaletteController.InvertColor (new Color (s.Rgb))));
				}

				h.Title.Text = i.Title;
				h.Image.ImageSource = new ImageIdToUrlConverter ().GetThumbnailUrl (
					i.ThumbnailImageId, 200.ToPx ());
				h.ImagePanel.MaxSize = new Size (i.ThumbnailWidth, i.ThumbnailHeight);

				h.ItemView.SetClick ((sender, e) => context.StartActivity (SnapshotActivity.NewIntent (i.Id)));
			}

			public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int position)
			{
				return new SnapshotViewHolder (View.Inflate (parent.Context, Resource.Layout.item_snapshot, null));
			}

			public override int ItemCount {
				get {
					return items.Count;
				}
			}

			#endregion

			class SnapshotViewHolder : RecyclerView.ViewHolder
			{
				public TextView Title { get; set; }

				public WebImageView Image { get; set; }

				public FixAspectFrameLayout ImagePanel { get; set; }

				public View TextPanel { get; set; }

				public bool JustCreated { get; set; }

				public SnapshotViewHolder (View convertView) : base (convertView)
				{
					Title = convertView.FindViewById<TextView> (Resource.Id.title);
					Image = convertView.FindViewById<WebImageView> (Resource.Id.image);
					ImagePanel = convertView.FindViewById<FixAspectFrameLayout> (Resource.Id.imagePanel);
					TextPanel = convertView.FindViewById<View> (Resource.Id.textPanel);
					JustCreated = true;

					var card = convertView.FindViewById<CardView> (Resource.Id.card);
					card.Radius = 2.ToPx ();
					ViewCompat.SetElevation (card, 10);
				}
			}
		}
	}
}