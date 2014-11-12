using Spectator.Core.Model;
using Android.OS;

namespace Spectator.Android.Application.Model
{
	public class AndroidPlatformEnvironment : PlatformEnvironment
	{
		public override bool SupportWebp {
			get {
				return Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr2;
			}
		}
	}
}