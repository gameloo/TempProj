using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                if (connector_1.PositionOnCanvas.X < connector_2.PositionOnCanvas.X) return Orientation.HorizontalRightLeft;
                else return Orientation.HorizontalLeftRight;
            }
            else if (connector_2.PositionOnElement == Position.right && connector_1.PositionOnElement == Position.left)
            {
                if (connector_2.PositionOnCanvas.X < connector_1.PositionOnCanvas.X) return Orientation.HorizontalRightLeft;
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
                if (connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y) return Orientation.VerticalBottomTop;
                else return Orientation.VerticalTopBottom;
            }
            else if (connector_2.PositionOnElement == Position.bottom && connector_1.PositionOnElement == Position.top)
            {
                if (connector_2.PositionOnCanvas.Y < connector_1.PositionOnCanvas.Y) return Orientation.VerticalBottomTop;
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


        public WireControl()
        {
            this.InitializeComponent();
        }

        public WireControl(ConnectorControl connector_1, ConnectorControl connector_2)
        {
            this.InitializeComponent();

            connector_1.update += Update;
            connector_2.update += Update;

            this.connector_1 = connector_1;
            this.connector_2 = connector_2;
            Update();

        }

        private void Update()
        {
            switch (GetOrientaion())
            {
                case Orientation.HorizontalRightLeft:
                    {
                        double leftPoint = connector_1.PositionOnCanvas.X < connector_2.PositionOnCanvas.X ? connector_1.PositionOnCanvas.X : connector_2.PositionOnCanvas.X;
                        Line_2.StartPoint = new Point(leftPoint + Math.Abs(connector_1.PositionOnCanvas.X - connector_2.PositionOnCanvas.X) / 2, connector_1.PositionOnCanvas.Y);
                        Line_2.EndPoint = new Point(Line_2.StartPoint.X, connector_2.PositionOnCanvas.Y);
                        break;
                    }
                case Orientation.HorizontalLeftRight:
                    {
                        double topPoint = connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y ? connector_1.PositionOnCanvas.Y : connector_2.PositionOnCanvas.Y;
                        Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint + Math.Abs(connector_1.PositionOnCanvas.Y - connector_2.PositionOnCanvas.Y) / 2);
                        Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                        break;
                    }
                case Orientation.HorizontalLeftLeft:
                    {

                        if (connector_1.PositionOnCanvas.Y == connector_2.PositionOnCanvas.Y)
                        {
                            Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, connector_1.PositionOnCanvas.Y + 60);
                            Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, connector_2.PositionOnCanvas.Y + 60);
                            break;
                        }
                        else
                        {
                            double topPoint = connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y ? connector_1.PositionOnCanvas.Y : connector_2.PositionOnCanvas.Y;
                            double bottomPoint = connector_1.PositionOnCanvas.Y > connector_2.PositionOnCanvas.Y ? connector_1.PositionOnCanvas.Y : connector_2.PositionOnCanvas.Y;

                            if (connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y)
                            {
                                if (connector_1.PositionOnCanvas.X < connector_2.PositionOnCanvas.X)
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, bottomPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                                else
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                            }
                            else
                            {
                                if (connector_1.PositionOnCanvas.X < connector_2.PositionOnCanvas.X)
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                                else
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, bottomPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                            }
                        }
                        break;
                    }
                case Orientation.HorizontalRightRight:
                    {

                        if (connector_1.PositionOnCanvas.Y == connector_2.PositionOnCanvas.Y)
                        {
                            Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, connector_1.PositionOnCanvas.Y + 60);
                            Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, connector_2.PositionOnCanvas.Y + 60);
                            break;
                        }
                        else
                        {
                            double topPoint = connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y ? connector_1.PositionOnCanvas.Y : connector_2.PositionOnCanvas.Y;
                            double bottomPoint = connector_1.PositionOnCanvas.Y > connector_2.PositionOnCanvas.Y ? connector_1.PositionOnCanvas.Y : connector_2.PositionOnCanvas.Y;

                            if (connector_1.PositionOnCanvas.Y > connector_2.PositionOnCanvas.Y)
                            {
                                if (connector_1.PositionOnCanvas.X < connector_2.PositionOnCanvas.X)
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, bottomPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                                else
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                            }
                            else
                            {
                                if (connector_1.PositionOnCanvas.X < connector_2.PositionOnCanvas.X)
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                                else
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, bottomPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                            }
                        }
                        break;
                    }
                case Orientation.VerticalBottomTop:
                    {
                        double topPoint = connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y ? connector_1.PositionOnCanvas.Y : connector_2.PositionOnCanvas.Y;
                        Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint + Math.Abs(connector_1.PositionOnCanvas.Y - connector_2.PositionOnCanvas.Y) / 2);
                        Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                        break;
                    }
                case Orientation.VerticalTopBottom:
                    {
                        if (connector_1.PositionOnCanvas.X == connector_2.PositionOnCanvas.X)
                        {
                            Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X + 60, connector_1.PositionOnCanvas.Y);
                            Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X + 60, connector_2.PositionOnCanvas.Y);
                        }
                        else
                        {
                            double leftPoint = connector_1.PositionOnCanvas.X < connector_2.PositionOnCanvas.X ? connector_1.PositionOnCanvas.X : connector_2.PositionOnCanvas.X;
                            Line_2.StartPoint = new Point(leftPoint + Math.Abs(connector_1.PositionOnCanvas.X - connector_2.PositionOnCanvas.X) / 2, connector_1.PositionOnCanvas.Y);
                            Line_2.EndPoint = new Point(Line_2.StartPoint.X, connector_2.PositionOnCanvas.Y);

                        }
                        break;

                    }
                case Orientation.VerticalTopTop:
                    {
                        if (connector_1.PositionOnCanvas.X == connector_2.PositionOnCanvas.X)
                        {
                            Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X + 60, connector_1.PositionOnCanvas.Y);
                            Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X + 60, connector_2.PositionOnCanvas.Y);
                        }
                        else
                        {
                            double topPoint = connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y ? connector_1.PositionOnCanvas.Y : connector_2.PositionOnCanvas.Y;

                            if (connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y)
                            {
                                if (connector_1.PositionOnCanvas.X > connector_2.PositionOnCanvas.X)
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                                else
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                            }
                            else
                            {
                                if (connector_1.PositionOnCanvas.X > connector_2.PositionOnCanvas.X)
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                                else
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, topPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                            }
                        }
                        break;
                    }
                case Orientation.VerticalBottomBottom:
                    {
                        if (connector_1.PositionOnCanvas.X == connector_2.PositionOnCanvas.X)
                        {
                            Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X + 60, connector_1.PositionOnCanvas.Y);
                            Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X + 60, connector_2.PositionOnCanvas.Y);
                        }
                        else
                        {
                            double bottomPoint = connector_1.PositionOnCanvas.Y > connector_2.PositionOnCanvas.Y ? connector_1.PositionOnCanvas.Y : connector_2.PositionOnCanvas.Y;

                            if (connector_1.PositionOnCanvas.Y < connector_2.PositionOnCanvas.Y)
                            {
                                if (connector_1.PositionOnCanvas.X > connector_2.PositionOnCanvas.X)
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, bottomPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                                else
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, bottomPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                            }
                            else
                            {
                                if (connector_1.PositionOnCanvas.X > connector_2.PositionOnCanvas.X)
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, bottomPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                                else
                                {
                                    Line_2.StartPoint = new Point(connector_1.PositionOnCanvas.X, bottomPoint);
                                    Line_2.EndPoint = new Point(connector_2.PositionOnCanvas.X, Line_2.StartPoint.Y);
                                }
                            }
                        }
                        break;
                    }

            }

            Bindings.Update();
        }

        private void Path_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Line_2.StartPoint = new Point(Line_2.StartPoint.X + e.Delta.Translation.X, Line_2.StartPoint.Y + e.Delta.Translation.Y);
            Line_2.EndPoint = new Point(Line_2.EndPoint.X + e.Delta.Translation.X, Line_2.EndPoint.Y + e.Delta.Translation.Y);
            Bindings.Update();
        }

        private void Path_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }

        private void LinePath_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            Bindings.Update();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            (parent as Panel).Children.Remove(this);
        }
    }
}

