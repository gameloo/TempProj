using EWB_GUI_Alpha.ElectronicComponents.PropertiesOfComponents.Active.CurrentSource;
using EWB_GUI_Alpha.ElectronicComponents.PropertiesOfComponents.Active.VoltageCurrentSource;
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

namespace EWB_GUI_Alpha.ElectronicComponents.Active
{
    public sealed partial class UCVoltageCurrentSource : UserControl, IEComponent
    {
        private Point PinPosLeft = new Point(-10, 50);
        private Point PinPosRight = new Point(110, 50);
        private Point PinPosTop = new Point(50, -10);
        private Point PinPosBot = new Point(50, 110);
        private Point IndicatorPosVert = new Point(-5, 120);
        private Point IndicatorPosHor = new Point(0, 90);
        private Point ComponentNamePosVert = new Point(105, 20);
        private Point ComponentNamePosHor = new Point(45, -5);
        public double Angle { get; set; }
        public double AngleIndicator { get; set; }
        private Point PositionConnector_1 { get; set; }
        private Point PositionConnector_2 { get; set; }
        private Point PositionIndicator { get; set; }
        private Point PositionTbName { get; set; }

        public string IndicatorValue { get; set; } = "3В";
        public string ComponentName { get; set; } = "U";

        public Point CenterComponent
        {
            get { return new Point(ActualWidth / 2, this.ActualHeight / 2); }
        }
        public Point OldPositionComponentOnCanvas { get; set; }


        public UCVoltageCurrentSource()
        {
            this.InitializeComponent();
            PositionIndicator = IndicatorPosHor;
            PositionTbName = ComponentNamePosHor;
            PositionConnector_1 = PinPosLeft;
            PositionConnector_2 = PinPosRight;
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

        // Поворот на 90 градусов по часовой стрелке
        private void Rotate90Clockwice(object sender, RoutedEventArgs e)
        {
            Angle += Angle != 270 ? 90 : -270;
            RotateComponent();
        }

        // Поворот на 90 градусов против часовой стрелки
        private void Rotate90Counterclockwice(object sender, RoutedEventArgs e)
        {
            Angle -= Angle != 0 ? 90 : -270;
            RotateComponent();
        }

        private void RotateComponent()
        {
            switch (Angle)
            {
                case 0:
                    {
                        PositionConnector_1 = PinPosLeft;
                        connector_1.PositionOnElement = Position.left;
                        PositionConnector_2 = PinPosRight;
                        connector_2.PositionOnElement = Position.right;

                        AngleIndicator = 0;
                        PositionIndicator = IndicatorPosHor;
                        PositionTbName = ComponentNamePosHor;
                        break;
                    }
                case 90:
                    {
                        PositionConnector_1 = PinPosTop;
                        connector_1.PositionOnElement = Position.top;
                        PositionConnector_2 = PinPosBot;
                        connector_2.PositionOnElement = Position.bottom;

                        AngleIndicator = -90;
                        PositionIndicator = IndicatorPosVert;
                        PositionTbName = ComponentNamePosVert;
                        break;
                    }
                case 180:
                    {
                        PositionConnector_1 = PinPosRight;
                        connector_1.PositionOnElement = Position.right;
                        PositionConnector_2 = PinPosLeft;
                        connector_2.PositionOnElement = Position.left;

                        AngleIndicator = 0;
                        PositionIndicator = IndicatorPosHor;
                        PositionTbName = ComponentNamePosHor;
                        break;
                    }
                case 270:
                    {
                        PositionConnector_1 = PinPosBot;
                        connector_1.PositionOnElement = Position.bottom;
                        PositionConnector_2 = PinPosTop;
                        connector_2.PositionOnElement = Position.top;

                        AngleIndicator = -90;
                        PositionIndicator = IndicatorPosVert;
                        PositionTbName = ComponentNamePosVert;
                        break;
                    }
            }
            Bindings.Update();
            ChildrenPositionUpdate();
        }

        private async void OpenProperties(object sender, RoutedEventArgs e)
        {
            var dialog = new CDVoltageCurrentSourceProperties(this);
            var result = await dialog.ShowAsync();
            Bindings.Update();
        }
    }
}
