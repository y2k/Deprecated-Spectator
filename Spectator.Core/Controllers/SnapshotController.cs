using System;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using System.Collections.Generic;

namespace Spectator.Core.Controllers
{
	public class SnapshotController
	{
		IEnumerable<Attachment> Attachments { get; set; }

		SnapshotModel model;
		Snapshot snapshot;

		public string Title { get; set; }

		public Action ReloadUi { get; set; }

		public SnapshotController (int snapshotId)
		{
			model = new SnapshotModel (snapshotId);
			Initialize ();
		}

		async void Initialize ()
		{
			snapshot = await model.Get ();
			Attachments = await model.GetAttachments ();
			ReloadUi ();
		}

		public class AttachmentController
		{
			public Uri Image { get; set; }

			public RelayCommand SelectAttachmentCommand { get; set; }

			public AttachmentController ()
			{
				SelectAttachmentCommand = new RelayCommand (() => {
					// TODO
				});
			}
		}
	}
}