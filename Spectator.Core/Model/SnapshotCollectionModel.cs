using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Spectator.Core.Model.Exceptions;
using Spectator.Core.Model.Web;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.Core.Model
{
	public class SnapshotCollectionModel : ISnapshotCollectionModel
	{
		private IWebConnect web = ServiceLocator.Current.GetInstance<IWebConnect> ();

		#region ISnapshotCollectionModel implementation

		public Task<IEnumerable<object>> GetAllAsync (int subscriptionId)
		{
			return Task.Run<IEnumerable<object>> (() => {
//				new ManualResetEvent(false).WaitOne(2000);
//				if (new Random().Next(1) == 0) throw new WrongAuthException();

				web.LoadSnapshots(subscriptionId);
				return new object[] { "1", "2", "3", "4", "5", "6", "7", "8" };
			});
		}

		#endregion
	}
}