
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Drawing;

namespace Spectator.Android.Application.Widget
{
	public class FixAspectFrameLayout : ViewGroup
	{
		private static readonly Size EMPTY = new Size (1, 1);
		private Size _size = EMPTY;

		public FixAspectFrameLayout (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public FixAspectFrameLayout (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		public Size MaxSize {
			get { return _size; }
			set {
				if (_size != value) {
					_size = value;
					RequestLayout ();
				}
			}
		}

		#region implemented abstract members of ViewGroup

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			int w = MeasureSpec.GetSize (widthMeasureSpec);
			int h = (int)(((float)w / _size.Width) * _size.Height);

			SetMeasuredDimension (w, h);
			int mw = MeasureSpec.MakeMeasureSpec (w, MeasureSpecMode.Exactly);
			int mh = MeasureSpec.MakeMeasureSpec (h, MeasureSpecMode.Exactly);
			for (int i = 0; i < ChildCount; i++) {
				GetChildAt (i).Measure (mw, mh);
			}
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			for (int i = 0; i < ChildCount; i++) {
				GetChildAt (i).Layout (0, 0, r - l, b - t);
			}
		}

		#endregion

		private void Initialize ()
		{
		}
	}
}