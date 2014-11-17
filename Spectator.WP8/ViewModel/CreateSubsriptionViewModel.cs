using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;
using Spectator.WP8.Model;
using Spectator.WP8.ViewModel.Base;
using Spectator.WP8.ViewModel.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Spectator.WP8.ViewModel
{
    public class CreateSubscriptionViewModel : BaseViewModel
    {
        string _title;
        public string Title { get { return _title; } set { Set(ref _title, value); } }

        string _link;
        public string Link { get { return _link; } set { Set(ref _link, value); } }

        bool _isBusy;
        public bool IsBusy { get { return _isBusy; } set { Set(ref _isBusy, value); } }

        public ObservableCollection<RssItemViewModel> RssItems { get; } = new ObservableCollection<RssItemViewModel>();

        public RelayCommand CreateCommand { get; set; }
        public RelayCommand ExportRssCommand { get; set; }

        public CreateSubscriptionViewModel()
        {
            CreateCommand = new RelayCommand(CreateSubscription);
            ExportRssCommand = new RelayCommand(ExportRss);

            Link = "http://yande.re/"; // TODO: 
        }

        #region Export RSS

        private void ExportRss()
        {
            if (CheckInputValidForExportRss())
                ProcessExportRss();
        }

        bool CheckInputValidForExportRss()
        {
            if (!Uri.IsWellFormedUriString(Link, UriKind.Absolute))
            {
                ShowError("You must set valid URL for extract RSS");
                return false;
            }
            return true;
        }

        async void ProcessExportRss()
        {
            IsBusy = true;
            try
            {
                var rss = await new RssExtractor(new Uri(Link)).ExtracRss();
                if (rss.Length == 0) ShowInformation("Not RSS found");
                UpdateRssItems(rss);
            }
            catch
            {
                ShowError("Export RSS is failed");
            }
            IsBusy = false;
        }

        void UpdateRssItems(RssExtractor.RssItem[] rss)
        {
            RssItems.ReplaceAll(rss.Select(s => new RssItemViewModel(this, s)));
        }

        public class RssItemViewModel
        {
            public RelayCommand SelectCommand { get; set; }

            public string Title { get; set; }

            public RssItemViewModel(CreateSubscriptionViewModel host, RssExtractor.RssItem item)
            {
                Title = item.Title;
                SelectCommand = new RelayCommand(() =>
                {
                    host.Title = item.Title;
                    host.Link = item.Link.AbsoluteUri;
                });
            }
        }

        #endregion

        #region Create new subscription

        void CreateSubscription()
        {
            if (CheckInputValidForCreateSubscription())
                ProcessCreateSubscription();
        }

        bool CheckInputValidForCreateSubscription()
        {
            if (string.IsNullOrEmpty(Title))
            {
                ShowError("Your must set title for subscription");
                return false;
            }
            if (!Uri.IsWellFormedUriString(Link, UriKind.Absolute))
            {
                ShowError("Your must set valid URL for subscription");
                return false;
            }
            return true;
        }

        async void ProcessCreateSubscription()
        {
            IsBusy = true;
            try
            {
                //await new SubscriptionModel().CreateNew(new Uri(Link), Title);
                await Task.Delay(2000);
                ShowInformation("Subscription created");
                NavigateBack();
            }
            catch
            {
                ShowError("Can't create subscription");
            }
            IsBusy = false;
        }

        #endregion

        void ShowError(string message)
        {
            new ErrorToast(message).Show();
        }

        void ShowInformation(string message)
        {
            new InformationToast(message).Show();
        }
    }
}