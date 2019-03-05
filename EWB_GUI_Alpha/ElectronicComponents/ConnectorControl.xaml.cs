using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace EWB_GUI_Alpha.ElectronicComponents
{
    public enum Position
    {
        top,
        bottom,
        left,
        right,
        center
    }


    public sealed partial class ConnectorControl : UserControl
    {
        // Private static fields
        private static bool isClick = false;
        private static ConnectorControl ConnectedConnectorProperty;
        //*

        public delegate void Update();
        public Update update;

        // Test /!\

        public static readonly DependencyProperty PosProperty =
            DependencyProperty.Register(
                "PositionOnElement", typeof(Position),
                typeof(ConnectorControl), null
            );
        public Position PositionOnElement
        {
            get { return (Position)GetValue(PosProperty); }
            set { SetValue(PosProperty, value); }
        }

        // *

        private Point point;
        public Point PositionOnCanvas
        {
            get
            {
                return point;
            }
            private set
            {
                point = value;
            }
        }

        private Func<Point> GetPositionOnCanvas;

        public void UpdatePositionOnCanvas()
        {
            point = GetPositionOnCanvas();
            update?.Invoke();
        }

        public ConnectorControl()
        {
            this.InitializeComponent();

            GetPositionOnCanvas = () =>
            {
                return new Point(
                    Canvas.GetLeft(
                        (UIElement)VisualTreeHelper.GetParent(
                            VisualTreeHelper.GetParent(this))
                        ) + Canvas.GetLeft(this) + this.ActualHeight / 2,
                    Canvas.GetTop(
                        (UIElement)VisualTreeHelper.GetParent(
                            VisualTreeHelper.GetParent(this))
                                ) + Canvas.GetTop(this) + this.ActualWidth / 2
                    );
            };
        }

        public ConnectorControl(Point positionOnCanvas)
        {
            this.InitializeComponent();

            GetPositionOnCanvas = () =>
            {
                return new Point(
                    Canvas.GetLeft(this) + this.ActualHeight / 2,
                    Canvas.GetTop(this) + this.ActualWidth / 2
                    );
            };

            this.PositionOnElement = Position.center;

            Canvas.SetLeft(this, positionOnCanvas.X - 10);
            Canvas.SetTop(this, positionOnCanvas.Y - 10);


        }

        private void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!isClick)
            {
                ConnectedConnectorProperty = this;
                isClick = true;
            }
            else
            {
                if (!ConnectedConnectorProperty.Equals(this))
                {
                    DependencyObject parent1 = VisualTreeHelper.GetParent(this);
                    DependencyObject parent = VisualTreeHelper.GetParent(parent1);
                    DependencyObject canvas = VisualTreeHelper.GetParent(parent);
                    (canvas as Panel).Children.Add(
                        new WireControl(ConnectedConnectorProperty, this)
                        );
                }
                isClick = false;
            }
        }

    }
}
