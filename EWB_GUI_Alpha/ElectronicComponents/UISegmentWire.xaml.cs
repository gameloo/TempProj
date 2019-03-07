using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace EWB_GUI_Alpha.ElectronicComponents
{
    public sealed partial class UISegmentWire : UserControl
    {
        public static readonly DependencyProperty StartPointDP =
            DependencyProperty.Register(
                "StartPoint", typeof(Point),
                typeof(UISegmentWire), null
            );
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointDP); }
            set { SetValue(StartPointDP, value); }
        }

        public static readonly DependencyProperty EndPointDP =
            DependencyProperty.Register(
                "EndPoint", typeof(Point),
                typeof(UISegmentWire), null
            );
        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointDP); }
            set { SetValue(EndPointDP, value); }
        }

        public Point CursorPosition { get; private set; }
        Dictionary<ConnectorControl, double> connectors;

        public UISegmentWire()
        {
            this.InitializeComponent();
            this.PointerPressed += new PointerEventHandler(OnPressed);
            connectors = new Dictionary<ConnectorControl, double>();
        }

        public void Update()
        {
            if (connectors.Count != 0)
            {
                foreach (var pair in connectors)
                {
                    Canvas.SetLeft(pair.Key, StartPoint.X + (EndPoint.X - StartPoint.X - 20) * pair.Value);
                    Canvas.SetTop(pair.Key, StartPoint.Y + (EndPoint.Y - StartPoint.Y - 20) * pair.Value);

                    if (StartPoint.X == EndPoint.X)
                    {
                        Canvas.SetLeft(pair.Key, StartPoint.X - 10);
                    }
                    if (StartPoint.Y == EndPoint.Y)
                    {
                        Canvas.SetTop(pair.Key, StartPoint.Y - 10);
                    }
                    pair.Key.update?.Invoke();
                }
            }

            Bindings.Update();
        }

        private void MenuFlyoutItem_AddConnector(object sender, RoutedEventArgs e)
        {
            var tempConnector = new ConnectorControl() { PositionOnElement = Position.bottom };
            container.Children.Add(tempConnector);

            if (StartPoint.X == EndPoint.X)
            {
                Canvas.SetLeft(tempConnector, StartPoint.X - 10);
                Canvas.SetTop(tempConnector, CursorPosition.Y - 10);
            }
            if (StartPoint.Y == EndPoint.Y)
            {
                Canvas.SetLeft(tempConnector, CursorPosition.X - 10);
                Canvas.SetTop(tempConnector, StartPoint.Y - 10);
            }
            else
            {
                Canvas.SetLeft(tempConnector, CursorPosition.X - 10);
                Canvas.SetTop(tempConnector, CursorPosition.Y - 10);
            }

            double d1 = Math.Sqrt(
                Math.Pow(StartPoint.X - EndPoint.X, 2) +
                Math.Pow(StartPoint.Y - EndPoint.Y, 2));
            double d2 = Math.Sqrt(
                Math.Pow(StartPoint.X - tempConnector.PositionCleatOnCanvas.X, 2) +
                Math.Pow(StartPoint.Y - tempConnector.PositionCleatOnCanvas.Y, 2));
            double ratio = d2 / d1;
            connectors.Add(tempConnector, ratio);

        }

        private void MenuFlyoutItem_Remove(object sender, RoutedEventArgs e)
        {
            DeleteElement();
        }

        public delegate void Delete();
        public Delete delete;

        public void DeleteElement()
        {
            if (connectors.Count != 0)
            {
                foreach (var pair in connectors)
                {
                    pair.Key.delete?.Invoke();
                }
            }
            delete?.Invoke();
        }

        void OnPressed(object sender, PointerRoutedEventArgs e)
        {
            CursorPosition = e.GetCurrentPoint(container).Position;
        }
    }
}
