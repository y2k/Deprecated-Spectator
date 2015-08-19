using System;
using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;

namespace Spectator.Core.ViewModels
{
    public class CreateSubscriptionViewModel : ViewModel
    {
        #region Properties

        string _title;

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        string _link;

        public string Link
        {
            get { return _link; }
            set { Set(ref _link, value); }
        }

        bool _titleError;

        public bool TitleError
        {
            get { return _titleError; }
            set { Set(ref _titleError, value); }
        }

        bool _linkError;

        public bool LinkError
        {
            get { return _linkError; }
            set { Set(ref _linkError, value); }
        }

        bool _inProgress;

        public bool InProgress
        {
            get { return _inProgress; }
            set { Set(ref _inProgress, value); }
        }

        public RelayCommand CreateCommand { get; set; }

        #endregion

        public CreateSubscriptionViewModel()
        {
            CreateCommand = new Command(Create);
        }

        async void Create()
        {
            if (!ValidCreateData())
                return;

            InProgress = true;
            try
            {
                await new SubscriptionModel().CreateNew(new Uri(Link), Title);
            }
            catch
            {
            }
            InProgress = false;
        }

        bool ValidCreateData()
        {
            LinkError = TitleError = false;
            LinkError = !Uri.IsWellFormedUriString(Link, UriKind.Absolute);
            TitleError = string.IsNullOrWhiteSpace(Title);
            return !LinkError && !TitleError;
        }

        public void Initialize(ExtractRssViewModel.NavigateToCreateSubscription argument)
        {
            Title = argument.Title;
            Link = argument.Link;
        }
    }
}