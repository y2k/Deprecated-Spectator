using System;
using System.Linq;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace Spectator.Core.Controllers
{
    [Obsolete]
    public class SnapshotController
    {
        public List<AttachmentController> Attachments { get; set; } = new List<AttachmentController>();

        public Action ReloadUi { get; set; }

        public bool IsImageLoad { get; set; }

        public string Title { get; set; }

        public string Created { get; set; }

        public string Image { get; set; }

        public bool HasContent { get; set; }

        public bool HasRevisions { get; set; }

        public string BaseUrl { get; set; }

        public string HtmlContent { get; set; }

        public bool IsDiffMode { get; set; }

        SnapshotModel model;
        Snapshot snapshot;

        public async Task ToggleDiffMode()
        {
            IsDiffMode = !IsDiffMode;
            HtmlContent = await (IsDiffMode ? model.GetDiff() : model.GetContent());
            ReloadUi();
        }

        public SnapshotController(int snapshotId)
        {
            model = new SnapshotModel(snapshotId);
        }

        public async Task Initialize()
        {
            await model.SyncWithWeb();
            snapshot = await model.Get();

            Created = "" + snapshot.Created;
            Image = new ImageIdToUrlConverter().Convert(snapshot.ThumbnailImageId);
            Title = snapshot.Title;
            HasContent = snapshot.HasWebContent;
            HasRevisions = snapshot.HasRevisions;

            Attachments = (await model.GetAttachments()).Select(s => new AttachmentController(s)).ToList();

            if (HasContent)
            {
                BaseUrl = snapshot.Source;
                HtmlContent = await model.GetContent();
            }

            ReloadUi();
        }

        public class AttachmentController
        {
            public Uri Image { get; set; }

            public RelayCommand SelectAttachmentCommand { get; set; }

            public AttachmentController(Attachment s)
            {
                SelectAttachmentCommand = new RelayCommand(() =>
                {
                    // TODO
                });

                // TODO: переделать установку размера миниатюры
                Image = CreateThumbnailUrl(new Uri(s.Image), 200);
            }

            Uri CreateThumbnailUrl(Uri url, int px)
            {
                var uri = string.Format(
                              "http://remote-cache.api-i-twister.net/Cache/Get?maxHeight=500&width={0}&url={1}",
                              px, Uri.EscapeDataString("" + url));
                return new Uri(uri);
            }
        }
    }
}