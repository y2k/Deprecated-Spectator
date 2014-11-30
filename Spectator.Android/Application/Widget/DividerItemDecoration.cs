using Android.Support.V7.Widget;
using Android.Graphics;

namespace Spectator.Android.Application.Widget
{
	public class DividerItemDecoration : RecyclerView.ItemDecoration
	{
		readonly int padding;

		public DividerItemDecoration (float paddingDip)
		{
			padding = (int)(App.Current.Resources.DisplayMetrics.Density * paddingDip);
		}

		public override void GetItemOffsets (Rect outRect, int position, RecyclerView parent)
		{
			outRect.Set (padding, padding, padding, padding);
		}
	}
}