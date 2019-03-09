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
    public delegate void UpdateComponentPosition();
    public delegate void DeleteComponent();

    public interface IEComponent
    {
        Point CenterComponent { get; }
        Point OldPositionComponentOnCanvas { get; set; }

        void RotateComponent(object sender, RoutedEventArgs e);
        void ChildrenPositionUpdate();
    }


    public static class IEComponentExtend
    {
        private static Point GetCenterComponentOnCanvas(UIElement element)
        {
            var CurrentPositionComponentOnCanvas = CustomVisualTreeHelper.PositionElementOnKernelCanvas(element);
            return new Point(
                (element as IEComponent).CenterComponent.X + CurrentPositionComponentOnCanvas.X,
                (element as IEComponent).CenterComponent.Y + CurrentPositionComponentOnCanvas.Y
                );
        }

        public static void UserControl_ManipulationStarted(this IEComponent component, object sender, ManipulationStartedRoutedEventArgs e)
        {
            (sender as IEComponent).OldPositionComponentOnCanvas = CustomVisualTreeHelper.PositionElementOnKernelCanvas(sender as UIElement);
        }

        public static void UserControl_ManipulationDelta(this IEComponent component, object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var left = Canvas.GetLeft((UIElement)sender) + e.Delta.Translation.X;
            var top = Canvas.GetTop((UIElement)sender) + e.Delta.Translation.Y;
            Canvas.SetLeft((UIElement)sender, left);
            Canvas.SetTop((UIElement)sender, top);
            (sender as IEComponent).ChildrenPositionUpdate();          
        }

        public static void UserControl_ManipulationCompleted(this IEComponent component, object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var currentPositionComponentOnCanvas = CustomVisualTreeHelper.PositionElementOnKernelCanvas(sender as UIElement);
            var left = Math.Round(currentPositionComponentOnCanvas.X); // Убрать Round?
            var top = Math.Round(currentPositionComponentOnCanvas.Y);

            // Выравнивание элемента по центру
            var centerComponentOnCanvas = GetCenterComponentOnCanvas(sender as UIElement);

            var step = 20;
            var deltaX = Math.Round(centerComponentOnCanvas.X) % step;
            if (deltaX < step / 2)
                left -= deltaX;
            else left += (step - deltaX);

            var deltaY = Math.Round(centerComponentOnCanvas.Y) % step;
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

            if (IsIntersectOnCanvas(sender as UIElement))
            {
                Canvas.SetLeft((UIElement)sender, (sender as IEComponent).OldPositionComponentOnCanvas.X);
                Canvas.SetTop((UIElement)sender, (sender as IEComponent).OldPositionComponentOnCanvas.Y);
            }

            (sender as IEComponent).ChildrenPositionUpdate();
        }

        // Проверка на пересечение с другими элементами в Canvas
        public static bool IsIntersectOnCanvas(UIElement element)
        {
            var currentPos = CustomVisualTreeHelper.PositionElementOnKernelCanvas(element);
            var uIElement =
                CustomVisualTreeHelper.KernelCanvas.Children.
                Where(e =>
                    {
                        return (e is IEComponent) && (!e.Equals(element));
                    }).
                FirstOrDefault(e =>
                {
                    var currentPos_e = CustomVisualTreeHelper.PositionElementOnKernelCanvas(e);
                    return (
                    // Находится ли левый верхний угол элемента (element) в области элемента (e)
                    (currentPos.X >= currentPos_e.X) &&
                    (currentPos.X <= (currentPos_e.X + e.DesiredSize.Width)) &&
                    (currentPos.Y >= currentPos_e.Y) &&
                    (currentPos.Y <= (currentPos_e.Y + e.DesiredSize.Height)) ||
                    // *
                    // Находится ли нижний правый угол элемента (element) в области элемента (e)
                    (currentPos.X + element.DesiredSize.Width >= currentPos_e.X) &&
                    (currentPos.X + element.DesiredSize.Width <= (currentPos_e.X + e.DesiredSize.Width)) &&
                    (currentPos.Y + element.DesiredSize.Height >= currentPos_e.Y) &&
                    (currentPos.Y + element.DesiredSize.Height <= (currentPos_e.Y + e.DesiredSize.Height)) ||
                    // *
                    (currentPos.X + element.DesiredSize.Width >= currentPos_e.X) &&
                    (currentPos.X <= (currentPos_e.X + e.DesiredSize.Width)) &&
                    (currentPos.Y + element.DesiredSize.Height >= currentPos_e.Y) &&
                    (currentPos.Y <= (currentPos_e.Y + e.DesiredSize.Height))
                    );
                });

            return uIElement == null ? false : true;
        }
    }
}

