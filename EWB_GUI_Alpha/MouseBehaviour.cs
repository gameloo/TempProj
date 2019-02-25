using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.UI.Xaml;

namespace EWB_GUI_Alpha
{
    public class MouseBehaviour : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
            "MouseY", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

        public double MouseY
        {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }

        public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
            "MouseX", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

        public double MouseX
        {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }

        protected override void OnAttached()
        {
            ////AssociatedObject.PointerMoved += AssociatedObjectOnMouseMove;
            //AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            ///var pos = mouseEventArgs.GetPosition(AssociatedObject);
            ///MouseX = pos.X;
            ///MouseY = pos.Y;
        }

        protected override void OnDetaching()
        {
            ///AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }
    }
}
