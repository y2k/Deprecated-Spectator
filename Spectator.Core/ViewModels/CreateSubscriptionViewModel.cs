using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;
using System;

namespace Spectator.Core.ViewModels
{
    public class CreateSubscriptionViewModel : ViewModelBase
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
            CreateCommand = new RelayCommand(OnClickedCreateSubscriptions);
        }

        void OnClickedCreateSubscriptions()
        {
            if (ValidCreateData())
                CreateSubscription();
        }

        bool ValidCreateData()
        {
            LinkError = TitleError = false;
            LinkError = !Uri.IsWellFormedUriString(Link, UriKind.Absolute);
            TitleError = string.IsNullOrWhiteSpace(Title);
            return !LinkError && !TitleError;
        }

        async void CreateSubscription()
        {
            InProgress = true;
            try { await new SubscriptionModel().CreateNew(new Uri(Link), Title); }
            catch { }
            InProgress = false;
        }
    }
}