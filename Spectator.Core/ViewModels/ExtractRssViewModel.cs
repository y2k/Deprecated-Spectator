using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;

namespace Spectator.Core.ViewModels
{
    public class ExtractRssViewModel : ViewModel
    {
        #region Properties

        public ObservableCollection<RssItemViewModel> RssItems { get; } = new ObservableCollection<RssItemViewModel>();

        string _link;

        public string Link
        {
            get { return _link; }
            set { Set(ref _link, value); }
        }

        bool _linkError;

        public bool LinkError
        {
            get { return _linkError; }
            set { Set(ref _linkError, value); }
        }

        bool _inProgress;

        public bool IsBusy
        {
            get { return _inProgress; }
            set { Set(ref _inProgress, value); }
        }

        public RelayCommand ExtractCommand { get; set; }

        #endregion

        public ExtractRssViewModel()
        {
            ExtractCommand = new RelayCommand(Extract);
        }

        async void Extract()
        {
            if (!ValidRssData())
                return;

            IsBusy = true;
            RssItems.Clear();

            var extractor = new RssExtractor(new Uri(Link));
            try
            {
                InitializeList(await extractor.ExtracRss());
            }
            catch
            {
            }
            IsBusy = false;
        }

        bool ValidRssData()
        {
            LinkError = false;
            LinkError = !Uri.IsWellFormedUriString(Link, UriKind.Absolute);
            return !LinkError;
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