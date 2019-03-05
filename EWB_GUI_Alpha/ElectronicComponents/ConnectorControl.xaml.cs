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

        public delegate void Delete();
        public Delete delete;

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

        public Point PositionCleatOnCanvas
        {
            get
            {
                return new Point(
                    CustomVisualTreeHelper.PositionElementOnKernelCanvas(this).X + this.ActualHeight / 2,
                    CustomVisualTreeHelper.PositionElementOnKernelCanvas(this).Y + this.ActualWidth / 2
                    );
            }
        }

        public ConnectorControl()
        {
            this.InitializeComponent(); 
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
                    CustomVisualTreeHelper.KernelCanvas.Children.Add(new WireControl(ConnectedConnectorProperty, this));
                }
                isClick = false;
            }
        }

    }
}
