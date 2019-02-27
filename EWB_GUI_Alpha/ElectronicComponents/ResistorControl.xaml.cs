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

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace EWB_GUI_Alpha.ElectronicComponents
{
    public partial class ResistorControl : UserControl, IEComponent
    {
        private Point PositionConnector_1 { get; set; } = new Point(0, 55);
        private Point PositionConnector_2 { get; set; } = new Point(110, 55);
        private Point PositionResistanceIndicator { get; set; } = new Point(50, 20);

        public Point CenterComponentOnCanvas
        {
            get
            {
                return new Point(
                    Canvas.GetLeft(this) + this.ActualHeight / 2,
                    Canvas.GetTop(this) + this.ActualWidth / 2
                    );
            }
        }

        public PointCollection CurrentPositionComponentOnCanvas
        {
            get
            {
                return new PointCollection()
                {
                    new Point(
                        Canvas.GetLeft(this),
                        Canvas.GetTop(this)
                        ),
                    new Point(
                        Canvas.GetLeft(this) + this.ActualHeight,
                        Canvas.GetTop(this) + this.ActualWidth
                        )
                };
            }
        }

        public PointCollection OldPositionComponentOnCanvas { get; set; }

        public Point PositionOnCanvas
        {
            get
            {
                return new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            }
        }

        public double Angle { get; set; }

        public double ResistanceValue { get; set; }


        public ResistorControl()
        {
            this.InitializeComponent();
        }

        public ResistorControl(ResistorControl resistorControl)
        {
            this.InitializeComponent();
            this.Angle = resistorControl.Angle;
            this.ResistanceValue = resistorControl.ResistanceValue;
        }

        public void RotateComponent(object sender, RoutedEventArgs e)
        {
            if (Angle == 0)
            {
                Angle = 90;
                PositionConnector_1 = new Point(55, 0);
                PositionConnector_2 = new Point(55, 110);
                PositionResistanceIndicator = new Point(90, 50);
            }
            else
            {
                Angle = 0;
                PositionConnector_1 = new Point(0, 55);
                PositionConnector_2 = new Point(110, 55);
                PositionResistanceIndicator = new Point(50, 20);
            }
            Bindings.Update();
            ChildrenPositionUpdate();
        }

        private void DeleteElement(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            (parent as Panel).Children.Remove(this);
        }

        public void ChildrenPositionUpdate()
        {
            connector_1.UpdatePositionOnCanvas(); //! Test
            connector_2.UpdatePositionOnCanvas(); //! Test
        }
    }
}
