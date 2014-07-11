using NUnit.Framework;
using System;
using Spectator.Core.Model.Web;
using System.Threading;

namespace Spectator.Core.Tests.Common
{
	public class MockWebConnect : IWebConnect
	{
		#region IWebConnect implementation

		public T Get<T> (string url)
		{
			Thread.Sleep (1000);
			return (T)Activator.CreateInstance(typeof(T));
		}

		public void PostWebForm (string url, params object[] formKeyValues)
		{
			Thread.Sleep (1000);
		}

		#endregion
	}
}