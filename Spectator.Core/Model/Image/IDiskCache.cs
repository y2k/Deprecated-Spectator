using System;
using System.IO;

namespace Spectator.Core.Model.Image
{
	public interface IDiskCache
	{
		object Get(Uri uri);

		void Put(Uri uri, Stream image);
	}
}