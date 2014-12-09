using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;
using System;
using System.Collections.ObjectModel;

namespace Spectator.Core.ViewModels
{
    class ExtractRssViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<RssItemViewModel> RssItems { get; } = new ObservableCollection<RssItemViewModel>();

        private string _link;
        public string Link
        {
            get { return _link; }
            set { Set(ref _link, value); }
        }

        private bool _linkError;
        public bool LinkError
        {
            get { return _linkError; }
            set { Set(ref _linkError, value); }
        }

        private bool _inProgress;
        public bool InProgress
        {
            get { return _inProgress; }
            set { Set(ref _inProgress, value); }
        }

        public RelayCommand ExtractCommand { get; set; }

        #endregion

        public ExtractRssViewModel()
        {
            ExtractCommand = new RelayCommand(OnClickExtractRss);
        }

        private void OnClickExtractRss()
        {
            if (ValidRssData())
                ExtractRss();
        }

        bool ValidRssData()
        {
            LinkError = !Uri.IsWellFormedUriString(Link, UriKind.Absolute);
            return !LinkError;
        }

        async void ExtractRss()
        {
            InProgress = true;
            RssItems.Clear();

            var extractor = new RssExtractor(new Uri(Link));
            try
            {
                InitializeList(await extractor.ExtracRss());
            }
            catch
            {
            }
            InProgress = false;
        }


        void InitializeList(RssExtractor.RssItem[] rssItems)
        {
            foreach (var s in rssItems)
                RssItems.Add(new RssItemViewModel { Title = s.Title, Link = "" + s.Link });
        }

        public class RssItemViewModel
        {
            public string Title { get; set; }

            public string Link { get; set; }
        }
    }
}