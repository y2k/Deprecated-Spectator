using System;
using Android.Content;
using Android.Util;
using Android.Views;

namespace Spectator.Android.Application.Widget
{
	public class GridPanel : ViewGroup
	{
		public GridPanel (Context context) :
			base (context)
		{
			Initialize ();
		}

		public GridPanel (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public GridPanel (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			// TODO: добавить проверку на changed
			int columns = ComputeAvailableColumns (r - l);
			var rows = ComputeRowCount (columns);
			var itemSize = (r - l) / columns;

			for (int y = 0; y < rows; y++)
				for (int x = 0; x < columns; x++) {
					int childIndex = x + y * columns;
					if (childIndex >= ChildCount)
						break;

					var child = GetChildAt (childIndex);
					child.Layout (x * itemSize, y * itemSize, (x + 1) * itemSize, (y + 1) * itemSize);
				}
		}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			CheckMeasureParams (widthMeasureSpec, heightMeasureSpec);

			int availableWidth = MeasureSpec.GetSize (widthMeasureSpec);
			int availableColumns = ComputeAvailableColumns (availableWidth);
			var itemSize = availableWidth / availableColumns;
			var measuredSize = CreateMeasuredSize (itemSize);

			for (int i = 0; i < ChildCount; i++)
				GetChildAt (i).Measure (measuredSize, measuredSize);

			var rows = ComputeRowCount (availableColumns);
			var requiredHeight = rows * itemSize;
			SetMeasuredDimension (availableWidth, requiredHeight);
		}

		void CheckMeasureParams (int widthMeasureSpec, int heightMeasureSpec)
		{
			// TODO: реализовать проверку
		}

		int ComputeAvailableColumns (int width)
		{
			return 3;
		}

		int CreateMeasuredSize (int size)
		{
			return MeasureSpec.MakeMeasureSpec (size, MeasureSpecMode.Exactly);
		}

		int ComputeRowCount (int availableColumns)
		{
			return (availableColumns - 1 + ChildCount) / availableColumns;
		}
	}
}