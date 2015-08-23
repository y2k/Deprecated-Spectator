using System;
using Spectator.Core.Model;
using System.Collections.ObjectModel;
using Spectator.Core.Model.Database;

namespace Spectator.Core.ViewModels
{
    public class SnapshotViewModel : ViewModel
    {
        #region Old fields

        public ObservableCollection<Attachment> Attachments { get; } = new ObservableCollection<Attachment>();

        string _title;

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        DateTime _create;

        public DateTime Created
        {
            get { return _create; }
            set { Set(ref _create, value); }
        }

        int _image;

        public int Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }

        bool _hasContent;

        public bool HasContent
        {
            get { return _hasContent; }
            set { Set(ref _hasContent, value); }
        }

        #endregion

        public string ContentUrl { get { return Get<string>(); } set { Set(value); } }

        public string DiffUrl { get { return Get<string>(); } set { Set(value); } }

        public string SourceUrl { get { return Get<string>(); } set { Set(value); } }

        SnapshotService service;

        public async void Initialize(SnapshotsViewModel.NavigateToSnapshotDetails argument)
        {
            service = new SnapshotService(argument.SnashotId);
            await service.SyncWithWeb();

            var snap = await service.Get();
            Title = snap.Title;
            Created = snap.Created;
            SourceUrl = snap.Source;

            ContentUrl = service.WebContent?.AbsoluteUri;
            DiffUrl = service.DiffContent?.AbsoluteUri;

            var attachs = await service.GetAttachments();
            if (attachs != null)
                Attachments.ReplaceAll(attachs);
        }
    }
}