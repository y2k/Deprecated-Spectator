using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using GalaSoft.MvvmLight.Helpers;
using Spectator.Droid.Activitis.Common;
using Spectator.Core.ViewModels;
using Spectator.Droid.Widgets;
using Spectator.Droid;

namespace Spectator.Droid.Activitis.Home
{
    public class SnapshotListFragment : BaseFragment
    {
        RecyclerView list;
        SwipeRefreshLayout refresh;
        View errorGeneral;
        View errorAuth;

        SnapshotsViewModel viewModel = new SnapshotsViewModel();

        #region Menu

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.snapshots, menu);
        }

        public  override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.delete)
                viewModel.DeleteCurrentSubscriptionCommand.Execute(null);
            return true;
        }

        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = HasOptionsMenu = true;

            MessengerInstance.Register<SnapshotsViewModel.NavigateToLoginMessage>(
                this, _ => StartActivity(new Intent(Activity, typeof(ProfileActivity))));
            MessengerInstance.Register<SnapshotsViewModel.NavigateToCreateSubscriptionMessage>(
                this, _ => new CreateSubscriptionFragment().Show(FragmentManager, null));
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            list.SetAdapter(new SnapshotAdapter(Activity, viewModel.Snapshots));

            viewModel
                .SetBinding(() => viewModel.IsAuthError, errorAuth, () => errorAuth.Visibility)
                .ConvertSourceToTarget(BooleanToVisibility);
            viewModel
                .SetBinding(() => viewModel.IsGeneralError, errorGeneral, () => errorGeneral.Visibility)
                .ConvertSourceToTarget(BooleanToVisibility);
            viewModel.SetBinding(() => viewModel.IsBusy, refresh, () => refresh.Refreshing);

            View.FindViewById(Resource.Id.createSubscription)
                .SetCommand("Click", viewModel.CreateSubscriptionCommand);
            errorAuth.SetCommand("Click", viewModel.LoginCommand);
            refresh.SetCommand("Refresh", viewModel.ReloadCommand);
        }

        static ViewStates BooleanToVisibility(bool s)
        {
            return s ? ViewStates.Visible : ViewStates.Gone;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var v = inflater.Inflate(Resource.Layout.fragment_snapshots, null);
            refresh = v.FindViewById<SwipeRefreshLayout>(Resource.Id.refresh);

            list = v.FindViewById<RecyclerView>(Resource.Id.list);
            list.SetLayoutManager(new StaggeredGridLayoutManager(2, StaggeredGridLayoutManager.Vertical));
            list.AddItemDecoration(new DividerItemDecoration(2));

            errorGeneral = v.FindViewById(Resource.Id.errorGeneral);
            errorAuth = v.FindViewById(Resource.Id.errorAuth);
            return v;
        }
    }
}