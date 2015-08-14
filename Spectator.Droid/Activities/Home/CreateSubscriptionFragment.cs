using System.Collections.Specialized;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Spectator.Core.ViewModels;
using System.Collections.Generic;
using Spectator.Droid;

namespace Spectator.Droid.Activitis.Home
{
    public class CreateSubscriptionFragment : DialogFragment
    {
        LinearLayout rssList;
        EditText title;
        EditText link;
        View rssButton;
        View okButton;
        View progress;

        CreateSubscriptionViewModel createViewModel = new CreateSubscriptionViewModel();
        ExtractRssViewModel extractViewModel = new ExtractRssViewModel();

        List<Binding> bindings = new List<Binding>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
            SetStyle(StyleNoTitle, 0);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            okButton.SetCommand("Click", createViewModel.CreateCommand);
            createViewModel.SetBinding(() => createViewModel.Link, link, () => link.Text, BindingMode.TwoWay);
            createViewModel.SetBinding(() => createViewModel.Title, title, () => title.Text, BindingMode.TwoWay);
            createViewModel
                .SetBinding(() => createViewModel.TitleError, title, () => title.Error)
                .ConvertSourceToTarget(ConvertFlagToErrorMessage);
            createViewModel
                .SetBinding(() => createViewModel.LinkError, link, () => link.Error)
                .ConvertSourceToTarget(ConvertFlagToErrorMessage);
            bindings.Add(
                createViewModel
                    .SetBinding(() => createViewModel.InProgress, progress, () => progress.Visibility)
                    .ConvertSourceToTarget(ConvertBoolToVisibility));

            rssButton.SetCommand("Click", extractViewModel.ExtractCommand);
            extractViewModel.SetBinding(() => extractViewModel.Link, link, () => link.Text, BindingMode.TwoWay);
            extractViewModel
                .SetBinding(() => extractViewModel.LinkError, link, () => link.Error)
                .ConvertSourceToTarget(ConvertFlagToErrorMessage);
            bindings.Add(
                extractViewModel
                    .SetBinding(() => extractViewModel.InProgress, progress, () => progress.Visibility)
                    .ConvertSourceToTarget(ConvertBoolToVisibility));
            extractViewModel.RssItems.CollectionChanged += HandleCollectionChanged;
        }

        string ConvertFlagToErrorMessage(bool error)
        {
            return error ? GetString(Resource.String.not_valid) : null;
        }

        static ViewStates ConvertBoolToVisibility(bool state)
        {
            return state ? ViewStates.Visible : ViewStates.Gone;
        }

        void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
                rssList.RemoveAllViews();
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newViews = e.NewItems.Cast<ExtractRssViewModel.RssItemViewModel>().Select(CreateRssView);
                foreach (var s in newViews)
                    rssList.AddView(s);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_create_subscription, null);
            link = v.FindViewById<EditText>(Resource.Id.link);
            title = v.FindViewById<EditText>(Resource.Id.title);
            rssButton = v.FindViewById(Resource.Id.rss);
            okButton = v.FindViewById(Resource.Id.ok);
            progress = v.FindViewById(Resource.Id.rssProgress);
            rssList = v.FindViewById<LinearLayout>(Resource.Id.rssList);
            return v;
        }

        void UpdateUiForCreateSubscriptionController()
        {
            var inProgress = createViewModel.InProgress;
            progress.Visibility = inProgress ? ViewStates.Visible : ViewStates.Gone;
            rssButton.Enabled = okButton.Enabled = !inProgress;
            rssList.Visibility = inProgress ? ViewStates.Gone : ViewStates.Visible;

            title.Error = createViewModel.TitleError ? GetString(Resource.String.required_field) : null;
            link.Error = createViewModel.LinkError ? GetString(Resource.String.not_valid_url) : null;
        }

        void UpdateUiForExtractController()
        {
            progress.Visibility = extractViewModel.InProgress ? ViewStates.Visible : ViewStates.Gone;
            rssButton.Enabled = okButton.Enabled = !extractViewModel.InProgress;
            link.Error = extractViewModel.LinkError ? GetString(Resource.String.not_valid_url) : null;

            rssList.RemoveAllViews();
            foreach (var s in extractViewModel.RssItems)
                rssList.AddView(CreateRssView(s));
        }

        View CreateRssView(ExtractRssViewModel.RssItemViewModel item)
        {
            var button = new Button(Activity) { Text = item.Title };
            button.Click += (sender, e) =>
            {
                title.Text = item.Title;
                link.Text = item.Link;
            };
            return button;
        }
    }
}