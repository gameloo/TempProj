using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace EWB_GUI_Alpha.ElectronicComponents
{
    public static class CustomVisualTreeHelper
    {
        public static Canvas KernelCanvas { get; set; }


        private static Point point = new Point(0,0);
        public static Point PositionElementOnKernelCanvas(UIElement element)
        {
            var parent = VisualTreeHelper.GetParent(element);
            if (parent is Canvas)
            {
                point = new Point ( point.X + Canvas.GetLeft(element), point.Y + Canvas.GetTop(element));
            }

            if (parent == null) return new Point(0, 0);

            if (parent.Equals(KernelCanvas))
            {
                var returnPoint = new Point(point.X, point.Y);
                point = new Point(0, 0);
                return returnPoint;
            }
            else return PositionElementOnKernelCanvas(parent as UIElement );
        }
    }
}
