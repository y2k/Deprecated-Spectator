using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Spectator.Core.Model;
using System;

namespace Spectator.Core.ViewModels
{
    public class CreateSubscriptionViewModel : ViewModelBase
    {
        #region Properties

        private string _title;
        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        private string _link;
        public string Link
        {
            get { return _link; }
            set { Set(ref _link, value); }
        }

        private bool _titleError;
        public bool TitleError
        {
            get { return _titleError; }
            set { Set(ref _titleError, value); }
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

        public RelayCommand CreateCommand { get; set; }

        #endregion

        public CreateSubscriptionViewModel()
        {
            CreateCommand = new RelayCommand(OnClickedCreateSubscriptions);
        }

        private void OnClickedCreateSubscriptions()
        {
            if (ValidCreateData())
                CreateSubscription();
        }

        private bool ValidCreateData()
        {
            LinkError = !Uri.IsWellFormedUriString(Link, UriKind.Absolute);
            TitleError = string.IsNullOrWhiteSpace(Title);
            return !LinkError && !TitleError;
        }

        private async void CreateSubscription()
        {
            InProgress = true;
            try
            {
                await new SubscriptionModel().CreateNew(new Uri(Link), Title);
            }
            catch { }
            InProgress = false;
        }
    }
}