using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Spectator.WP8.Views
{
    public class OneChildPanel : Grid
    {
        //private int currentDisplay;

        //public int CurrentDisplay
        //{
        //    get { return currentDisplay; }
        //    set { currentDisplay = value; ReloadCurentDisplay(); }
        //}

        public static readonly DependencyProperty CurrentDisplayProperty = DependencyProperty.Register("CurrentDisplay", typeof(int), typeof(OneChildPanel), new PropertyMetadata(0, CurrentDisplayCallback));

        private static void CurrentDisplayCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OneChildPanel)d).ReloadCurentDisplay();
        }

        public int CurrentDisplay
        {
            get { return (int)GetValue(CurrentDisplayProperty); }
            set { SetValue(CurrentDisplayProperty, value); }
        }

        public OneChildPanel()
        {
            Loaded += (sender, e) => ReloadCurentDisplay();
        }

        private void ReloadCurentDisplay()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Visibility = CurrentDisplay == i ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }
    }
}