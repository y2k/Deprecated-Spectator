using Spectator.Core.Model.Web;
using System;
using System.Threading;
using Spectator.Core.Model.Web.Proto;

namespace Spectator.Core.Tests.Common
{
	public class MockWebConnect : IApiClient
	{
		#region IWebConnect implementation

		public SnapshotsResponse.ProtoSnapshot GetSnapshot (int serverId)
		{
			throw new NotImplementedException ();
		}

		[Obsolete]
		public T Get<T> (string url)
		{
			Thread.Sleep (100);
			return (T)Activator.CreateInstance (typeof(T));
		}

		public void PostWebForm (string url, params object[] formKeyValues)
		{
			Thread.Sleep (100);
		}

		public SnapshotsResponse Get (int toId)
		{
			throw new NotImplementedException ();
		}

		public SnapshotsResponse Get (int subscriptionId, int toId)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}