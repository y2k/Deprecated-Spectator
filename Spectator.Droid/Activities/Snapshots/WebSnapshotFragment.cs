//using Spectator.Android.Application.Activity.Common.Base;
//using Android.Views;
//using Android.OS;
//using Android.Webkit;
//using Spectator.Core.Controllers;
//
//namespace Spectator.Android.Application.Activity.Snapshots
//{
//	public class WebSnapshotFragment : BaseFragment
//	{
//		WebView webview;
//
//		SnapshotController controller;
//
//		public async override void OnCreate (Bundle savedInstanceState)
//		{
//			base.OnCreate (savedInstanceState);
//			RetainInstance = true;
//			HasOptionsMenu = true;
//
//			controller = new SnapshotController (Arguments.GetInt ("id"));
//			await controller.Initialize ();
//		}
//
//		public override void OnActivityCreated (Bundle savedInstanceState)
//		{
//			base.OnActivityCreated (savedInstanceState);
//			controller.ReloadUi = HandleInvalidateUi;
//			HandleInvalidateUi ();
//		}
//
//		void HandleInvalidateUi ()
//		{
//			webview.LoadDataWithBaseURL (controller.BaseUrl, controller.HtmlContent, null, null, null);
//			Activity.SupportInvalidateOptionsMenu ();
//		}
//
//		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
//		{
//			inflater.Inflate (Resource.Menu.snapshot_web, menu);
//			menu.FindItem (Resource.Id.diff).SetVisible (controller.HasRevisions);
//			menu.FindItem (Resource.Id.diff).SetChecked (controller.IsDiffMode);
//			menu.FindItem (Resource.Id.loadImages).SetChecked (controller.IsImageLoad);
//		}
//
//		public override bool OnOptionsItemSelected (IMenuItem item)
//		{
//			switch (item.ItemId) {
//			case Resource.Id.diff:
//				controller.ToggleDiffMode ();
//				return true;
//			case Resource.Id.loadImages:
//				controller.IsImageLoad = !controller.IsImageLoad;
//				webview.Settings.LoadsImagesAutomatically = controller.IsImageLoad;
//				return true;
//			case Resource.Id.switchToInfo:
//				((SnapshotActivity)Activity).SwitchToInfo ();
//				return true;
//			}
//			return false;
//		}
//
//		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
//		{
//			webview = new WebView (Activity);
//			webview.Settings.JavaScriptEnabled = true;
//			webview.Settings.JavaScriptCanOpenWindowsAutomatically = false;
//
//			webview.Settings.UseWideViewPort = true;
//			webview.Settings.LoadWithOverviewMode = true;
//
//			webview.Settings.BuiltInZoomControls = true;
//			webview.Settings.DisplayZoomControls = false;
//
//			webview.Settings.LoadsImagesAutomatically = false;
//			return webview;
//		}
//	}
//}