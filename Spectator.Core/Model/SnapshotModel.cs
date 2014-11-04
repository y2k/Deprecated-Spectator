using System;
using Spectator.Core.Model.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.Core.Model
{
	public class SnapshotModel
	{
		IApiClient api = ServiceLocator.Current.GetInstance<IApiClient> ();
		IRepository storage = ServiceLocator.Current.GetInstance<IRepository> ();
		int snapshotId;

		public SnapshotModel (int snapshotId)
		{
			this.snapshotId = snapshotId;
		}

		public Uri WebContent { get; private set; }

		public Uri DiffContent { get; private set; }

		public Task Reload ()
		{
			return Task.Run (() => {
				var oldSnapshot = storage.GetSnapshot (snapshotId);
				var snapshot = api.GetSnapshot (oldSnapshot.ServerId);

				var newSnapshot = snapshot.ConvertToSnapshot (oldSnapshot.SubscriptionId);
				newSnapshot.Id = oldSnapshot.Id;

				storage.Update (newSnapshot);
			});
		}

		public Task<Snapshot> Get ()
		{
			return Task.Run (() => {
				var snapshot = storage.GetSnapshot (snapshotId);
				WebContent = snapshot.HasWebContent
					? new Uri (Constants.BaseApi, "/Content/Index/" + snapshot.ServerId)
					: null;
				DiffContent = snapshot.HasRevisions
					? new Uri (Constants.BaseApi, "/Content/Diff/" + snapshot.ServerId)
					: null;
				return snapshot;
			});
		}

		public Task<IEnumerable<Attachment>> GetAttachments ()
		{
			return Task.Run (() => storage.GetAttachements (snapshotId));
		}
	}
}