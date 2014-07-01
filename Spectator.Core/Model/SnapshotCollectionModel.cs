using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Spectator.Core.Model.Exceptions;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Web.Proto;
using System.Linq;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Model
{
	public class SnapshotCollectionModel : ISnapshotCollectionModel
	{
		private IWebConnect web = ServiceLocator.Current.GetInstance<IWebConnect> ();

		#region ISnapshotCollectionModel implementation

		public Task<IEnumerable<Snapshot>> GetAllAsync (int subscriptionId)
		{
			return Task.Run<IEnumerable<Snapshot>> (() => {
				var data = web.Get<ProtoSnapshotsResponse> ("http://debug.spectator.api-i-twister.net/api/snapshot2");
				return data.Snapshots.Select (s => new Snapshot{ Title = s.Title, ThumbnailImageId = s.Thumbnail }).ToList ();
			});
		}

		#endregion
	}
}