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
        private Point PositionConnector_1 { get; set; } = new Point(-10, 50);
        private Point PositionConnector_2 { get; set; } = new Point(110, 50);
        private Point PositionResistanceIndicator { get; set; } = new Point(50, 20);

        public Point CenterComponent
        {
            get { return new Point(ActualWidth / 2, this.ActualHeight / 2); }
        }
        public Point OldPositionComponentOnCanvas { get; set; }

        public double Angle { get; set; }
        public double ResistanceValue { get; set; }

        public ResistorControl()
        {
            this.InitializeComponent();
        }

        public void RotateComponent(object sender, RoutedEventArgs e)
        {
            if (Angle == 0)
            {
                Angle = 90;
                PositionConnector_1 = new Point(50, -10);
                connector_1.PositionOnElement = Position.top;
                PositionConnector_2 = new Point(50, 110);
                connector_2.PositionOnElement = Position.bottom;
                PositionResistanceIndicator = new Point(90, 50);
            }
            else
            {
                Angle = 0;
                PositionConnector_1 = new Point(-10, 50);
                connector_1.PositionOnElement = Position.left;
                PositionConnector_2 = new Point(110, 50);
                connector_2.PositionOnElement = Position.right;
                PositionResistanceIndicator = new Point(50, 20);
            }
            Bindings.Update();
            ChildrenPositionUpdate();
        }

        private void DeleteElement(object sender, RoutedEventArgs e)
        {
            connector_1.OnDeleteComponent?.Invoke();
            connector_2.OnDeleteComponent?.Invoke();
            CustomVisualTreeHelper.KernelCanvas.Children.Remove(this);
        }

        public void ChildrenPositionUpdate()
        {
            connector_1.ChildrenPositionUpdate();
            connector_2.ChildrenPositionUpdate();
        }
    }
}
