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

    //public enum Orientation
    //{
    //    HorizontalRightLeft,
    //    HorizontalLeftRight,
    //    HorizontalLeftLeft,
    //    HorizontalRightRight,
    //    VerticalBottomTop,
    //    VerticalTopBottom,
    //    VerticalBottomBottom,
    //    VerticalTopTop,
    //    Medley
    //}


    public sealed partial class WireControl : UserControl
    {

        private Point test_1 = new Point(10, 10);
        private Point test_2 = new Point(100, 100);

        private ConnectorControl connector_1;
        private ConnectorControl connector_2;

        //private ManipulationModes ManipulationModes
        //{
        //    get
        //    {
        //        if (GetOrientaion() == Orientation.HorizontalRightLeft ||
        //            GetOrientaion() == Orientation.VerticalTopBottom ||
        //             GetOrientaion() == Orientation.VerticalTopTop ||
        //              GetOrientaion() == Orientation.VerticalBottomBottom)
        //            return ManipulationModes.TranslateX;
        //        else return ManipulationModes.TranslateY;
        //    }

        //}

        //private Orientation GetOrientaion()
        //{
        //    if (connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.left)
        //    {
        //        if (connector_1.PositionCleatOnCanvas.X < connector_2.PositionCleatOnCanvas.X) return Orientation.HorizontalRightLeft;
        //        else return Orientation.HorizontalLeftRight;
        //    }
        //    else if (connector_2.PositionOnElement == Position.right && connector_1.PositionOnElement == Position.left)
        //    {
        //        if (connector_2.PositionCleatOnCanvas.X < connector_1.PositionCleatOnCanvas.X) return Orientation.HorizontalRightLeft;
        //        else return Orientation.HorizontalLeftRight;
        //    }
        //    else if (connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.left)
        //    {
        //        return Orientation.HorizontalLeftLeft;
        //    }
        //    else if (connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.right)
        //    {
        //        return Orientation.HorizontalRightRight;
        //    }
        //    else if (connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.top)
        //    {
        //        if (connector_1.PositionCleatOnCanvas.Y < connector_2.PositionCleatOnCanvas.Y) return Orientation.VerticalBottomTop;
        //        else return Orientation.VerticalTopBottom;
        //    }
        //    else if (connector_2.PositionOnElement == Position.bottom && connector_1.PositionOnElement == Position.top)
        //    {
        //        if (connector_2.PositionCleatOnCanvas.Y < connector_1.PositionCleatOnCanvas.Y) return Orientation.VerticalBottomTop;
        //        else return Orientation.VerticalTopBottom;
        //    }
        //    else if (connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.top)
        //    {
        //        return Orientation.VerticalTopTop;
        //    }
        //    else if (connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom)
        //    {
        //        return Orientation.VerticalBottomBottom;
        //    }
        //    else return Orientation.Medley;

        //}

        private Point CursorPosition { get; set; }
        // TEST

        void OnPressed(object sender, PointerRoutedEventArgs e)
        {
            CursorPosition = e.GetCurrentPoint(CustomVisualTreeHelper.KernelCanvas).Position;
            Bindings.Update();
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

            connector_1.update += Segment_2.Update;
            connector_2.update += Segment_2.Update;

            connector_1.update += Segment_3.Update;
            connector_2.update += Segment_3.Update;

            connector_1.delete += Segment_1.DeleteElement;
            connector_2.delete += Segment_1.DeleteElement;
            connector_1.delete += Segment_2.DeleteElement;
            connector_2.delete += Segment_2.DeleteElement;
            connector_1.delete += Segment_3.DeleteElement;
            connector_2.delete += Segment_3.DeleteElement;

            Segment_1.delete += DeleteElement;
            Segment_2.delete += DeleteElement;
            Segment_3.delete += DeleteElement;

            this.connector_1 = connector_1;
            this.connector_2 = connector_2;




            Update();
        }


        private void Update()
        {
            Action<UISegmentWire> SetPositionBot;
            Action<UISegmentWire> SetPositionTop;
            Action<UISegmentWire> SetPositionMidleVertical;
            Action<UISegmentWire> SetPositionMidleHorizontal;

            // C1 Выше
            if (connector_1.PositionCleatOnCanvas.Y < connector_2.PositionCleatOnCanvas.Y)
            {
                SetPositionBot = (segment) =>
                {
                    segment.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y);
                    segment.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y);
                };
                SetPositionTop = (segment) =>
                {
                    segment.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y);
                    segment.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y);
                };
                SetPositionMidleVertical = (segment) =>
                {
                    segment.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X + (connector_2.PositionCleatOnCanvas.X - connector_1.PositionCleatOnCanvas.X) / 2, connector_1.PositionCleatOnCanvas.Y);
                    segment.EndPoint = new Point(connector_1.PositionCleatOnCanvas.X + (connector_2.PositionCleatOnCanvas.X - connector_1.PositionCleatOnCanvas.X) / 2, connector_2.PositionCleatOnCanvas.Y);
                };
                SetPositionMidleHorizontal = (segment) =>
                {
                    segment.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y + (connector_2.PositionCleatOnCanvas.Y - connector_1.PositionCleatOnCanvas.Y) / 2);
                    segment.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y + (connector_2.PositionCleatOnCanvas.Y - connector_1.PositionCleatOnCanvas.Y) / 2);
                };

                // C1 Левее
                if (connector_1.PositionCleatOnCanvas.X < connector_2.PositionCleatOnCanvas.X)
                {
                    //  S2 on Bot
                    if (connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.bottom
                        )
                    {
                        SetPositionBot(Segment_2);
                    }
                    //  *
                    // S2 on Top
                    else
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.top
                        )
                    {
                        SetPositionTop(Segment_2);
                    }
                    //  *
                    //  S2 Midle vertical
                    else
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.bottom
                        )
                    {
                        SetPositionMidleVertical(Segment_2);
                    }
                    //  *
                    //  S2 Midle horizontal
                    else
                    {
                        SetPositionMidleHorizontal(Segment_2);
                    }
                }
                // C1 и C2 на одном уровне
                else if (connector_1.PositionCleatOnCanvas.X == connector_2.PositionCleatOnCanvas.X)
                {
                    //  Правый крюк
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.bottom
                        )
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X + 60, connector_1.PositionCleatOnCanvas.Y);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X + 60, connector_2.PositionCleatOnCanvas.Y);
                    }
                    else
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.left
                        )
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X - 60, connector_1.PositionCleatOnCanvas.Y);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X - 60, connector_2.PositionCleatOnCanvas.Y);
                    }
                    else
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y);
                    }
                }
                // С1 Правее
                else
                {

                    //  S2 on Bot
                    if (connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.right
                        )
                    {
                        SetPositionBot(Segment_2);
                    }
                    //  *
                    // S2 on Top
                    else
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.left
                        )
                    {
                        SetPositionTop(Segment_2);
                    }
                    //  *
                    //  S2 Midle vertical
                    else
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.bottom
                        )
                    {
                        SetPositionMidleVertical(Segment_2);
                    }
                    //  *
                    //  S2 Midle horizontal
                    else
                    {
                        SetPositionMidleHorizontal(Segment_2);
                    }

                }
            }
            // C1 и C2 на одном уровне
            else if (connector_1.PositionCleatOnCanvas.Y == connector_2.PositionCleatOnCanvas.Y)
            {
                // C1 Левее
                if (connector_1.PositionCleatOnCanvas.X < connector_2.PositionCleatOnCanvas.X)
                {
                    //  Bot
                    if (
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.right
                        )
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y + 60);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y + 60);
                    }
                    // Top
                    else if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.right
                        )
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y - 60);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y - 60);
                    }
                    else
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y);
                    }
                }
                // С1 Правее
                else
                {
                    //  Bot
                    if (
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.left
                        )
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y + 60);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y + 60);
                    }
                    // Top
                    else if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.left
                        )
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y - 60);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y - 60);
                    }
                    else
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y);
                    }
                }
            }
            // С1 ниже
            else
            {
                SetPositionBot = (segment) =>
                {
                    segment.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y);
                    segment.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y);
                };
                SetPositionTop = (segment) =>
                {
                    segment.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y);
                    segment.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y);
                };
                SetPositionMidleVertical = (segment) =>
                {
                    segment.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X + (connector_2.PositionCleatOnCanvas.X - connector_1.PositionCleatOnCanvas.X) / 2, connector_1.PositionCleatOnCanvas.Y);
                    segment.EndPoint = new Point(connector_1.PositionCleatOnCanvas.X + (connector_2.PositionCleatOnCanvas.X - connector_1.PositionCleatOnCanvas.X) / 2, connector_2.PositionCleatOnCanvas.Y);
                };
                SetPositionMidleHorizontal = (segment) =>
                {
                    segment.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y + (connector_1.PositionCleatOnCanvas.Y - connector_2.PositionCleatOnCanvas.Y) / 2);
                    segment.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y + (connector_1.PositionCleatOnCanvas.Y - connector_2.PositionCleatOnCanvas.Y) / 2);
                };
                // C1 Левее
                if (connector_1.PositionCleatOnCanvas.X < connector_2.PositionCleatOnCanvas.X)
                {
                    if (connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.right
                        )
                    {
                        SetPositionBot(Segment_2);
                    }
                    //  *
                    // S2 on Top
                    else
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.left
                        )
                    {
                        SetPositionTop(Segment_2);
                    }
                    //  *
                    //  S2 Midle vertical
                    else
                    if (
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.top
                        )
                    {
                        SetPositionMidleVertical(Segment_2);
                    }
                    //  *
                    //  S2 Midle horizontal
                    else
                    {
                        SetPositionMidleHorizontal(Segment_2);
                    }
                }
                // C1 и C2 на одном уровне
                else if (connector_1.PositionCleatOnCanvas.X == connector_2.PositionCleatOnCanvas.X)
                {
                    //  Правый крюк
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.top
                        )
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X + 60, connector_1.PositionCleatOnCanvas.Y);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X + 60, connector_2.PositionCleatOnCanvas.Y);
                    }
                    else
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.top
                        )
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X - 60, connector_1.PositionCleatOnCanvas.Y);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X - 60, connector_2.PositionCleatOnCanvas.Y);
                    }
                    else
                    {
                        Segment_2.StartPoint = new Point(connector_1.PositionCleatOnCanvas.X, connector_1.PositionCleatOnCanvas.Y);
                        Segment_2.EndPoint = new Point(connector_2.PositionCleatOnCanvas.X, connector_2.PositionCleatOnCanvas.Y);
                    }
                }
                // С1 Правее
                else
                {
                    if (connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.left ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.right
                        )
                    {
                        SetPositionBot(Segment_2);
                    }
                    //  *
                    // S2 on Top
                    else
                    if (
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.top && connector_2.PositionOnElement == Position.right ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.bottom ||
                        connector_1.PositionOnElement == Position.right && connector_2.PositionOnElement == Position.right
                        )
                    {
                        SetPositionTop(Segment_2);
                    }
                    //  *
                    //  S2 Midle vertical
                    else
                    if (
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.top ||
                        connector_1.PositionOnElement == Position.left && connector_2.PositionOnElement == Position.top
                        )
                    {
                        SetPositionMidleVertical(Segment_2);
                    }
                    //  *
                    //  S2 Midle horizontal
                    else
                    {
                        SetPositionMidleHorizontal(Segment_2);
                    }
                }

            }

            Segment_1.EndPoint = Segment_2.StartPoint;
            Segment_3.StartPoint = Segment_2.EndPoint;
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



        private void DeleteElement()
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

