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
    public sealed partial class CapacitorControl : UserControl, IEComponent
    {
        private Point PositionConnector_1 { get; set; } = new Point(-10, 50);
        private Point PositionConnector_2 { get; set; } = new Point(110, 50);
        private Point PositionTbName { get; set; } = new Point(45, 0);
        private Point PositionResistanceIndicator { get; set; } = new Point(40, 90);
        public double Angle { get; set; }
        public string ResistanceValue { get; set; } = "100";
        public string ComponentName { get; set; } = "C";

        public CapacitorControl()
        {
            this.InitializeComponent();
            connector_1.OnConnectWire += HideConnector_1;
            connector_2.OnConnectWire += HideConnector_2;
        }
        private void HideConnector_1()
        {
            connector_1.Visibility = Visibility.Collapsed;
        }

        private void HideConnector_2()
        {
            connector_2.Visibility = Visibility.Collapsed;
        }

        public Point CenterComponent
        {
            get { return new Point(ActualWidth / 2, this.ActualHeight / 2); }
        }

        public Point OldPositionComponentOnCanvas { get; set; }

        public void ChildrenPositionUpdate()
        {
            connector_1.ChildrenPositionUpdate();
            connector_2.ChildrenPositionUpdate();
        }

        public void RotateComponent(object sender, RoutedEventArgs e)
        {
            if (Angle == 0)
            {
                Angle = -90;
                PositionConnector_1 = new Point(50, -10);
                connector_1.PositionOnElement = Position.top;
                PositionConnector_2 = new Point(50, 110);
                connector_2.PositionOnElement = Position.bottom;
                PositionTbName = new Point(90, 20);
                PositionResistanceIndicator = new Point(0, 80);
            }
            else
            {
                Angle = 0;
                PositionConnector_1 = new Point(-10, 50);
                connector_1.PositionOnElement = Position.left;
                PositionConnector_2 = new Point(110, 50);
                connector_2.PositionOnElement = Position.right;
                PositionTbName = new Point(45, 0);
                PositionResistanceIndicator = new Point(40, 90);

            }
            Bindings.Update();
            ChildrenPositionUpdate();
        }

        private async void OpenProperties(object sender, RoutedEventArgs e)
        {
            //var dialog = new PropertiesOfResistorContentDialog(this);
            //var result = await dialog.ShowAsync();
            //Bindings.Update();
        }

        private void DeleteElement(object sender, RoutedEventArgs e)
        {
            connector_1.OnDeleteComponent?.Invoke();
            connector_2.OnDeleteComponent?.Invoke();
            CustomVisualTreeHelper.KernelCanvas.Children.Remove(this);
        }
    }
}
