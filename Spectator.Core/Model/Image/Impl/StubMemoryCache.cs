using System;

namespace Spectator.Core.Model.Image.Impl
{
	public class StubMemoryCache : IMemoryCache
	{
		public StubMemoryCache ()
		{
		}

		#region IMemoryCache implementation

		public object Get (Uri uri)
		{
			return null;
		}

		public void Put (Uri uri, object image)
		{
			//
		}

		#endregion
	}
}

