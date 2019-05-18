using EWB_GUI_Alpha.ElectronicComponents.PropertiesOfComponents.Passive.Resistor;
using EWB_GUI_Alpha.PropertiesOfComponents.Tools;
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

namespace EWB_GUI_Alpha.ElectronicComponents.Tools
{
    public sealed partial class UCVoltmeter : UserControl, IEComponent
    {
        private Point PinPosLeft = new Point(-10, 40);
        private Point PinPosRight = new Point(150, 40);
        private Point PinPosTop = new Point(70, -10);
        private Point PinPosBot = new Point(70, 90);
        private Point[] Pin1LineHoriz = new Point[] { new Point(0, 50), new Point(15, 50) };
        private Point[] Pin2LineHoriz = new Point[] { new Point(145, 50), new Point(160, 50) };

        private Point PositionConnector_1 { get; set; } = new Point(-10, 40);
        private Point PositionConnector_2 { get; set; } = new Point(150, 40);

        public string IndicateValue { get; set; } = "00В";

        public UCVoltmeter()
        {
            this.InitializeComponent();
            connector_1.OnConnectWire += HideConnector_1;
            connector_2.OnConnectWire += HideConnector_2;
            connector_1.OnDisconnectWire += ShowConnector_1;
            connector_2.OnDisconnectWire += ShowConnector_2;
        }

        private void HideConnector_1()
        {
            connector_1.Visibility = Visibility.Collapsed;
        }
        private void HideConnector_2()
        {
            connector_2.Visibility = Visibility.Collapsed;
        }
        private void ShowConnector_1()
        {
            connector_1.Visibility = Visibility.Visible;
        }
        private void ShowConnector_2()
        {
            connector_2.Visibility = Visibility.Visible;
        }

        public Point CenterComponent { get; set; } = new Point(100, 60);

        public Point OldPositionComponentOnCanvas { get; set; }

        public void ChildrenPositionUpdate()
        {
            connector_1.ChildrenPositionUpdate();
            connector_2.ChildrenPositionUpdate();
        }
        private void DeleteElement(object sender, RoutedEventArgs e)
        {
            connector_1.OnDeleteComponent?.Invoke();
            connector_2.OnDeleteComponent?.Invoke();
            CustomVisualTreeHelper.KernelCanvas.Children.Remove(this);
        }
        private bool OrientationHorisontal = true;
        public void RotateComponent(object sender, RoutedEventArgs e)
        {
            if (OrientationHorisontal)
            {
                OrientationHorisontal = false;
                PositionConnector_1 = PinPosTop;
                connector_1.PositionOnElement = Position.top;
                PositionConnector_2 = PinPosBot;
                connector_2.PositionOnElement = Position.bottom;

                Pin1LineHoriz = new Point[] { new Point(80, 0), new Point(80, 25) };
                Pin2LineHoriz = new Point[] { new Point(80, 75), new Point(80, 100) };
            }
            else
            {
                OrientationHorisontal = true;
                PositionConnector_1 = PinPosLeft;
                connector_1.PositionOnElement = Position.left;
                PositionConnector_2 = PinPosRight;
                connector_2.PositionOnElement = Position.right;

                Pin1LineHoriz = new Point[] { new Point(0, 50), new Point(15, 50) };
                Pin2LineHoriz = new Point[] { new Point(145, 50), new Point(160, 50) };
            }
            Bindings.Update();
            ChildrenPositionUpdate();
        }
        private async void OpenProperties(object sender, RoutedEventArgs e)
        {
            var dialog = new CDVoltageProp(this);
            var result = await dialog.ShowAsync();
            Bindings.Update();
        }
    }
}
