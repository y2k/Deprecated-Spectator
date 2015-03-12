using System.Collections.ObjectModel;
using System.Drawing;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Spectator.Android.Application.Activity.Common;
using Spectator.Android.Application.Activity.Snapshots;
using Spectator.Android.Application.Widget;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Bundle = global::Android.OS.Bundle;
using Color = global::Android.Graphics.Color;
using Size = System.Drawing.Size;

namespace Spectator.Android.Application.Activity.Home
{
    class SnapshotAdapter : RecyclerView.Adapter
    {
        PaletteController.Fabric paletteFabric = new PaletteController.Fabric();

        static readonly Color DEFAULT_BACKGROUND = new Color(0x57, 0xC2, 0xAD);
        static readonly Color DEFAULT_FOREGROUND = new Color(0x1D, 0x63, 0x5A);

        Context context;
        ObservableCollection<Snapshot> items;

        public SnapshotAdapter(Context context, ObservableCollection<Snapshot> items)
        {
            this.context = context;
            this.items = items;
            RegisterCollectionObservers();
        }

        void RegisterCollectionObservers()
        {
            items.CollectionChanged += (sender, e) => NotifyDataSetChanged();
        }

        #region implemented abstract members of Adapter

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var h = (SnapshotViewHolder)holder;
            var i = items[position];

            h.TextPanel.SetBackgroundColor(DEFAULT_BACKGROUND);
            h.Title.SetTextColor(DEFAULT_FOREGROUND);
            if (h.JustCreated)
            {
                var c = paletteFabric.NewInstance(h.Image);
                c.AddView(h.TextPanel, s => s.LightVibrantSwatch, (v, s) => v.SetBackgroundColor(new Color(s.Rgb)));
                c.AddView(h.Title, s => s.LightVibrantSwatch, (v, s) => v.SetTextColor(PaletteController.InvertColor(new Color(s.Rgb))));
            }

            h.Title.Text = i.Title;
            h.Image.ImageSource = new ImageIdToUrlConverter().GetThumbnailUrl(
                i.ThumbnailImageId, 200.ToPx());
            h.ImagePanel.MaxSize = new Size(i.ThumbnailWidth, i.ThumbnailHeight);

            h.ItemView.SetClick((sender, e) => context.StartActivity(SnapshotActivity.NewIntent(i.Id)));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int position)
        {
            return new SnapshotViewHolder(View.Inflate(parent.Context, Resource.Layout.item_snapshot, null));
        }

        public override int ItemCount
        {
            get
            {
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

            public SnapshotViewHolder(View convertView)
                : base(convertView)
            {
                Title = convertView.FindViewById<TextView>(Resource.Id.title);
                Image = convertView.FindViewById<WebImageView>(Resource.Id.image);
                ImagePanel = convertView.FindViewById<FixAspectFrameLayout>(Resource.Id.imagePanel);
                TextPanel = convertView.FindViewById<View>(Resource.Id.textPanel);
                JustCreated = true;

                var card = convertView.FindViewById<CardView>(Resource.Id.card);
                card.Radius = 2.ToPx();
                ViewCompat.SetElevation(card, 10);
            }
        }
    }
}