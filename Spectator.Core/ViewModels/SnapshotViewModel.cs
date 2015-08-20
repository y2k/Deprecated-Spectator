using System;
using System.Windows.Input;
using Spectator.Core.Model;

namespace Spectator.Core.ViewModels
{
    public class SnapshotViewModel : ViewModel
    {
        #region Old fields

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

        public string PreviewUrl { get { return Get<string>(); } set { Set(value); } }

        public string SourceUrl { get { return Get<string>(); } set { Set(value); } }

        public ICommand SetModeWebCommand { get; set; }

        public ICommand SetModeDiffCommand { get; set; }

        SnapshotService service;

        public SnapshotViewModel()
        {
            SetModeWebCommand = new Command(async () => PreviewUrl = await service.GetContent());
            SetModeDiffCommand = new Command(async () => PreviewUrl = await service.GetDiff());
        }

        public async void Initialize(SnapshotsViewModel.NavigateToSnapshotDetails argument)
        {
            service = new SnapshotService(argument.SnashotId);
            await service.SyncWithWeb();

            var snap = await service.Get();
            Title = snap.Title;
            Created = snap.Created;
            SourceUrl = snap.Source;
        }
    }
}