using System;
using System.Linq;
using Spectator.Core.Model.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;
using System.Net.Http;

namespace Spectator.Core.Model
{
	public class SnapshotModel
	{
		ISpectatorApi api = ServiceLocator.Current.GetInstance<ISpectatorApi> ();
		IRepository storage = ServiceLocator.Current.GetInstance<IRepository> ();
		int snapshotId;

		public SnapshotModel (int snapshotId)
		{
			this.snapshotId = snapshotId;
		}

		public Uri WebContent { get; private set; }

		public Uri DiffContent { get; private set; }

		ContentCache cache = new ContentCache ();

		public Task SyncWithWeb ()
		{
			return Task.Run (async () => {
				var oldSnapshot = storage.GetSnapshot (snapshotId);
				var snapshot = await api.GetSnapshot (oldSnapshot.ServerId);

				var newSnapshot = snapshot.ConvertToSnapshot (oldSnapshot.SubscriptionId);
				newSnapshot.Id = oldSnapshot.Id;

				storage.Update (newSnapshot);
				updateAttachments (snapshot.Images);

				await ReloadContent (newSnapshot);
			});
		}

		async Task ReloadContent (Snapshot snapshot)
		{
			if (snapshot.HasWebContent)
				await cache.SaveToCache (GetWebContentUrl (snapshot));
			if (snapshot.HasRevisions)
				await cache.SaveToCache (GetDiffUrl (snapshot));
		}

		public Task<string> GetContent ()
		{
			return cache.LoadFromCache (WebContent);
		}

		public Task<string> GetDiff ()
		{
			return cache.LoadFromCache (DiffContent);
		}

		void updateAttachments (List<string> images)
		{
			var attachments = images
				.Select (s => new Attachment { Image = s, SnapshotId = snapshotId })
				.ToList ();
			storage.ReplaceAll (attachments);
		}

		public Task<Snapshot> Get ()
		{
			return Task.Run (() => {
				var snapshot = storage.GetSnapshot (snapshotId);
				WebContent = snapshot.HasWebContent ? GetWebContentUrl (snapshot) : null;
				DiffContent = snapshot.HasRevisions ? GetDiffUrl (snapshot) : null;
				return snapshot;
			});
		}

		static Uri GetWebContentUrl (Snapshot snapshot)
		{
			return new Uri (Constants.BaseApi, "/Content/Index/" + snapshot.ServerId);
		}

		static Uri GetDiffUrl (Snapshot snapshot)
		{
			return new Uri (Constants.BaseApi, "/Content/Diff/" + snapshot.ServerId);
		}

		public Task<IEnumerable<Attachment>> GetAttachments ()
		{
			return Task.Run (() => storage.GetAttachements (snapshotId));
		}
	}
}