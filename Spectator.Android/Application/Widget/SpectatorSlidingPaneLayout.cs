using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Spectator.Android.Application.Widget
{
	public class SpectatorSlidingPaneLayout : SlidingPaneLayout
	{
		public SpectatorSlidingPaneLayout (Context context) :
			base (context)
		{
			Initialize ();
		}

		public SpectatorSlidingPaneLayout (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public SpectatorSlidingPaneLayout (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			//
			SetShadowResource (Resource.Drawable.shadow_right_drawable);
		}
	}
}