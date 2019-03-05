using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
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
    public enum Orientation
    {
        HorizontalRightLeft,
        HorizontalLeftRight,
        HorizontalLeftLeft,
        HorizontalRightRight,
        VerticalBottomTop,
        VerticalTopBottom,
        VerticalBottomBottom,
        VerticalTopTop,
        Medley
    }


    public sealed partial class WireControl : UserControl
    {
        private ConnectorControl connector_1;
        private ConnectorControl connector_2;

        private ManipulationModes ManipulationModes
        {
            get
            {
                if (GetOrientaion() == Orientation.HorizontalRightLeft ||
                    GetOrientaion() == Orientation.VerticalTopBottom ||
                     GetOrientaion() == Orientation.VerticalTopTop ||
                      GetOrientaion() == Orientation.VerticalBottomBottom)
                    return ManipulationModes.TranslateX;
                else return ManipulationModes.TranslateY;
            }

        }

        private Orientation GetOrientaion()
        {
            if (connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.left)
            {
                if (connector_1.PositionCleatOnCanvas.X < connector_2.PositionCleatOnCanvas.X) return Orientation.HorizontalRightLeft;
                else return Orientation.HorizontalLeftRight;
            }
            else if (connector_2.PositionOnElement == Position.right && connector_1.PositionOnElement == Position.left)
            {
                if (connector_2.PositionCleatOnCanvas.X < connector_1.PositionCleatOnCanvas.X) return Orientation.HorizontalRightLeft;
                else return Orientation.HorizontalLeftRight;
            }
            else if (connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.left)
            {
                return Orientation.HorizontalLeftLeft;
            }
            else if (connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.right)
            {
                return Orientation.HorizontalRightRight;
            }
            else if (connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.top)
            {
                if (connector_1.PositionCleatOnCanvas.Y < connector_2.PositionCleatOnCanvas.Y) return Orientation.VerticalBottomTop;
                else return Orientation.VerticalTopBottom;
            }
            else if (connector_2.PositionOnElement == Position.bottom && connector_1.PositionOnElement == Position.top)
            {
                if (connector_2.PositionCleatOnCanvas.Y < connector_1.PositionCleatOnCanvas.Y) return Orientation.VerticalBottomTop;
                else return Orientation.VerticalTopBottom;
            }
            else if (connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.top)
            {
                return Orientation.VerticalTopTop;
            }
            else if (connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom)
            {
                return Orientation.VerticalBottomBottom;
            }
            else return Orientation.Medley;

        }

        private Point CursorPosition { get; set; }
        // TEST

        void OnPressed(object sender, PointerRoutedEventArgs e)
        {
            CursorPosition = e.GetCurrentPoint(CustomVisualTreeHelper.KernelCanvas).Position;
        }

        // %
        public WireControl()
        {
            this.InitializeComponent();
            this.PointerPressed += new PointerEventHandler(OnPressed);
        }

        public WireControl(ConnectorControl connector_1, ConnectorControl connector_2)
        {
            this.InitializeComponent();
            this.PointerPressed += new PointerEventHandler(OnPressed);


            connector_1.update += Update;
            connector_2.update += Update;

            connector_1.update += Segment_1.Update;
            connector_2.update += Segment_1.Update;


            connector_1.delete += Segment_1.DeleteElement;
            connector_2.delete += Segment_1.DeleteElement;

            connector_1.delete += DeleteElement;
            connector_2.delete += DeleteElement;


            this.connector_1 = connector_1;
            this.connector_2 = connector_2;

            Update();
        }

        private void Update()
        {
            Bindings.Update();
        }

        private void Path_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {  
            Bindings.Update();
        }

        private void Path_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }

        private void LinePath_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            Bindings.Update();
        }

        public void DeleteElement()
        {
            CustomVisualTreeHelper.KernelCanvas.Children.Remove(this);
        }


        private void MenuFlyoutItem_AddConnector(object sender, RoutedEventArgs e)
        {
            var connector = new ConnectorControl() { PositionOnElement = Position.center };
            CustomVisualTreeHelper.KernelCanvas.Children.Add(connector);
            Canvas.SetLeft(connector, CursorPosition.X - 10);
            Canvas.SetTop(connector, CursorPosition.Y - 10);
        }
    }
}

