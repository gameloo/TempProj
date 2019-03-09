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
    public sealed partial class WireControl : UserControl
    {

        private Point test_1 = new Point(10, 10);
        private Point test_2 = new Point(100, 100);

        private ConnectorControl connector_1;
        private ConnectorControl connector_2;

        private Point CursorPosition { get; set; }
        private UpdateComponentPosition UpdateSegmentsPosition { get; set; }

        public WireControl()
        {
            this.InitializeComponent();
        }

        public WireControl(ConnectorControl connector_1, ConnectorControl connector_2)
        {
            this.InitializeComponent();

            this.connector_1 = connector_1;
            this.connector_2 = connector_2;

            UpdateSegmentsPosition += Update;
            UpdateSegmentsPosition += Segment_1.Update;
            UpdateSegmentsPosition += Segment_2.Update;
            UpdateSegmentsPosition += Segment_3.Update;

            connector_1.OnChangeElementPosition += UpdateSegmentsPosition;
            connector_2.OnChangeElementPosition += UpdateSegmentsPosition;
            connector_1.OnDeleteComponent += DeleteElement;
            connector_2.OnDeleteComponent += DeleteElement;

            Segment_1.OnClickDeleteSegmentWire += DeleteElement;
            Segment_2.OnClickDeleteSegmentWire += DeleteElement;
            Segment_3.OnClickDeleteSegmentWire += DeleteElement;

            UpdateSegmentsPosition();
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
                    if(connector_1.PositionOnElement == Position.center && connector_2.PositionOnElement == Position.center)
                    {
                        SetPositionMidleHorizontal(Segment_2);
                    }
                    //  S2 on Bot
                    else
                    if (connector_1.PositionOnElement == Position.center && connector_2.PositionOnElement != Position.right ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.bottom ||
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
                    if(connector_1.PositionOnElement == Position.center && connector_2.PositionOnElement == Position.center)
                    {
                        SetPositionMidleHorizontal(Segment_2);
                    }
                    //  S2 on Bot
                    else
                    if (connector_1.PositionOnElement == Position.center && connector_2.PositionOnElement != Position.left ||
                        connector_1.PositionOnElement == Position.bottom && connector_2.PositionOnElement == Position.top ||
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

        private void DeleteElement()
        {
            CustomVisualTreeHelper.KernelCanvas.Children.Remove(this);
        }
    }
}

