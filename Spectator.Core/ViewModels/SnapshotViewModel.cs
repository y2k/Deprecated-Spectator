using System;
using GalaSoft.MvvmLight;

namespace Spectator.Core.ViewModels
{
    public class SnapshotViewModel : ViewModelBase
    {
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

        public void Initialize(int snapshotId)
        {
            //
        }
    }
}