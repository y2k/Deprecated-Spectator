//using Android.OS;
//using Android.Views;
//using Android.Widget;
//using Spectator.Android.Application.Activity.Common.Base;
//using Spectator.Android.Application.Widget;
//using Spectator.Core.Controllers;
//
//namespace Spectator.Android.Application.Activity.Snapshots
//{
//	public class ContentSnapshotFragment : BaseFragment
//	{
//		TextView title;
//		TextView created;
//		WebImageView image;
//		GridPanel attachments;
//
//		SnapshotController controller;
//
//		public async override void OnCreate (Bundle savedInstanceState)
//		{
//			base.OnCreate (savedInstanceState);
//			RetainInstance = true;
//			HasOptionsMenu = true;
//			controller = new SnapshotController (Arguments.GetInt ("id"));
//			await controller.Initialize ();
//		}
//
//		public override void OnCreateOptionsMenu (IMenu menu, MenuInflater inflater)
//		{
//			inflater.Inflate (Resource.Menu.snapshot, menu);
//			menu.FindItem (Resource.Id.switchToWeb).SetVisible (controller.HasContent);
//		}
//
//		public override bool OnOptionsItemSelected (IMenuItem item)
//		{
//			switch (item.ItemId) {
//			case Resource.Id.switchToWeb:
//				((SnapshotActivity)Activity).SwitchToWeb ();
//				return true;
//			}
//			return false;
//		}
//
//		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
//		{
//			View v = inflater.Inflate (Resource.Layout.fragment_snapshot_content, null);
//			title = v.FindViewById<TextView> (Resource.Id.title);
//			created = v.FindViewById<TextView> (Resource.Id.created);
//			image = v.FindViewById<WebImageView> (Resource.Id.image);
//			attachments = v.FindViewById<GridPanel> (Resource.Id.attachments);
//			return v;
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
//			title.Text = controller.Title;
//			created.Text = controller.Created;
//			image.ImageSource = controller.Image;
//
//			attachments.RemoveAllViews ();
//			foreach (var a in controller.Attachments)
//				attachments.AddView (CreateAttachmentView (a));
//
//			Activity.SupportInvalidateOptionsMenu ();
//		}
//
//		View CreateAttachmentView (SnapshotController.AttachmentController a)
//		{
//			var view = new WebImageView (Activity) {
//				ImageSource = "" + a.Image,
//			};
//			view.SetScaleType (ImageView.ScaleType.CenterCrop);
//			return view;
//		}
//	}
//}