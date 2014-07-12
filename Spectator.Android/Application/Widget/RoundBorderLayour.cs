﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Spectator.Android.Application.Widget
{
	public class RoundBorderLayour : FrameLayout
	{
		private readonly int[] lastLayout = new int[2];

		private Paint clipPaint;
		private Canvas clipCanvas;
		private Bitmap canvasBitmap;
		private RectF rect;

		public RoundBorderLayour (Context context, IAttributeSet attrs) : base (context, attrs)
		{
			clipPaint = new Paint { AntiAlias = true };
		}

		protected override void OnLayout (bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout (changed, left, top, right, bottom);

			int w = right - left;
			int h = bottom - top;
			if (lastLayout [0] != w && lastLayout [1] != h) {
				if (w > 0 && h > 0) {
					canvasBitmap = Bitmap.CreateBitmap (w, h, Bitmap.Config.Argb8888);
					clipCanvas = new Canvas (canvasBitmap);
					rect = new RectF (0, 0, w, h);
					clipPaint.SetShader(new BitmapShader (canvasBitmap, BitmapShader.TileMode.Clamp, BitmapShader.TileMode.Clamp));
				} else {
					clipCanvas = null;
					canvasBitmap = null;
					clipPaint = null;
					rect = null;
				}
			}
		}

		protected override void DispatchDraw (Canvas canvas)
		{
			if (clipCanvas != null) {
				base.DispatchDraw (clipCanvas);
				canvas.DrawOval (rect, clipPaint);
			}
		}
	}
}