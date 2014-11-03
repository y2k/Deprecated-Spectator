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

				var localSnapshot = GetSnapshotFromRepository ();
				var snapshot = api.GetSnapshot (localSnapshot.ServerId);
				localSnapshot = snapshot.ConvertToSnapshot (localSnapshot.SubscriptionId);

				throw new NotImplementedException ();

			});
		}

		Snapshot GetSnapshotFromRepository ()
		{
			throw new NotImplementedException ();
		}

		public Task<Snapshot> Get ()
		{
			throw new NotImplementedException ();
		}

		public Task<IEnumerable<Attachment>> GetAttachments ()
		{
			throw new NotImplementedException ();
		}
	}
}