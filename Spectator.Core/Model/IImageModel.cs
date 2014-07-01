using System;
using System.Threading.Tasks;
using Spectator.Core.Model.Image;

namespace Spectator.Core.Model
{
	public interface IImageModel
	{
		void Load(object token, Uri originalUri, int maxWidth, Action<ImageWrapper> imageCallback);
	}
}