﻿using System;

namespace Spectator.Core.Model.Image
{
	public class ImageWrapper : IDisposable
	{
		public object Image { get; set; }

		#region IDisposable implementation

		public void Dispose ()
		{
			// TODO Заглушка для LRUCache
		}

		#endregion
	}
}