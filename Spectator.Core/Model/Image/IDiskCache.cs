using System;
using System.IO;

namespace Spectator.Core.Model.Image
{
	public interface IDiskCache
	{
		ImageWrapper Get(Uri uri);

		void Put(Uri uri, Stream image);
	}
}