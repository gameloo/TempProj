using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace EWB_GUI_Alpha.ElectronicComponents
{
    public interface IEComponent
    {
        Point CenterComponentOnCanvas { get; }
        PointCollection CurrentPositionComponentOnCanvas { get; }
        PointCollection OldPositionComponentOnCanvas { get; set; }

        void RotateComponent(object sender, RoutedEventArgs e);
        void ChildrenPositionUpdate();
    }


    public static class IEComponentExtend
    {
        public static void UserControl_ManipulationStarted(this IEComponent component, object sender, ManipulationStartedRoutedEventArgs e)
        {
            ((IEComponent)sender).OldPositionComponentOnCanvas = ((IEComponent)sender).CurrentPositionComponentOnCanvas;
        }

        public static void UserControl_ManipulationDelta(this IEComponent component, object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var left = Canvas.GetLeft((UIElement)sender) + e.Delta.Translation.X;
            var top = Canvas.GetTop((UIElement)sender) + e.Delta.Translation.Y;
            Canvas.SetLeft((UIElement)component, left);
            Canvas.SetTop((UIElement)component, top);
            ((IEComponent)sender).ChildrenPositionUpdate();
        }

        public static void UserControl_ManipulationCompleted(this IEComponent component, object sender, ManipulationCompletedRoutedEventArgs e) 
        {
            var left = Math.Round((sender as IEComponent).CurrentPositionComponentOnCanvas[0].X); // Убрать Round?
            var top = Math.Round((sender as IEComponent).CurrentPositionComponentOnCanvas[0].Y);

            // Выравнивание элемента по центру
            var step = 20;
            var deltaX = Math.Round((sender as IEComponent).CenterComponentOnCanvas.X) % step;
            if (deltaX < step / 2)
                left -= deltaX;
            else left += (step - deltaX);

            var deltaY = Math.Round((sender as IEComponent).CenterComponentOnCanvas.Y) % step;
            if (deltaY < step / 2)
                top -= deltaY;
            else top += (step - deltaY);
            // *
            // Проверка на выход из границ Canvas
            left = left < 0 ? 0 : left;
            top = top < 0 ? 0 : top;
            // *

            Canvas.SetLeft((UIElement)sender, left);
            Canvas.SetTop((UIElement)sender, top);

            if (IsIntersectOnCanvas((IEComponent)sender))
            {
                Canvas.SetLeft((UIElement)sender, ((IEComponent)sender).OldPositionComponentOnCanvas[0].X);
                Canvas.SetTop((UIElement)sender, ((IEComponent)sender).OldPositionComponentOnCanvas[0].Y);
            }

            ((IEComponent)sender).ChildrenPositionUpdate();
        }

        // Проверка на пересечение с другими элементами в Canvas
        public static bool IsIntersectOnCanvas(IEComponent element)
        {
            var currentPos = element.CurrentPositionComponentOnCanvas;

            DependencyObject parent = VisualTreeHelper.GetParent((UIElement)element);
            var uIElement =
                (parent as Panel).Children.
                Where(e =>
                    {
                        return (e is IEComponent) && (!e.Equals(element));
                    }).
                FirstOrDefault(e =>
                {
                    return (
                    // Находится ли левый верхний угол элемента (element) в области элемента (e)
                    (currentPos[0].X >= ((IEComponent)e).CurrentPositionComponentOnCanvas[0].X) &&
                    (currentPos[0].X <= ((IEComponent)e).CurrentPositionComponentOnCanvas[1].X) &&
                    (currentPos[0].Y >= ((IEComponent)e).CurrentPositionComponentOnCanvas[0].Y) &&
                    (currentPos[0].Y <= ((IEComponent)e).CurrentPositionComponentOnCanvas[1].Y) ||
                    // *
                    // Находится ли нижний правый угол элемента (element) в области элемента (e)
                    (currentPos[1].X >= ((IEComponent)e).CurrentPositionComponentOnCanvas[0].X) &&
                    (currentPos[1].X <= ((IEComponent)e).CurrentPositionComponentOnCanvas[1].X) &&
                    (currentPos[1].Y >= ((IEComponent)e).CurrentPositionComponentOnCanvas[0].Y) &&
                    (currentPos[1].Y <= ((IEComponent)e).CurrentPositionComponentOnCanvas[1].Y) ||
                    // *
                    (currentPos[1].X >= ((IEComponent)e).CurrentPositionComponentOnCanvas[0].X) &&
                    (currentPos[0].X <= ((IEComponent)e).CurrentPositionComponentOnCanvas[1].X) &&
                    (currentPos[1].Y >= ((IEComponent)e).CurrentPositionComponentOnCanvas[0].Y) &&
                    (currentPos[0].Y <= ((IEComponent)e).CurrentPositionComponentOnCanvas[1].Y)
                    );
                });

            if (uIElement == null) return false;
            else return true;
        }
    }
}

