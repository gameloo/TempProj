using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class UISegmentWire : UserControl
    {
        public static readonly DependencyProperty StartPointDP =
            DependencyProperty.Register(
                "StartPoint", typeof(Point),
                typeof(UISegmentWire), null
            );
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointDP); }
            set { SetValue(StartPointDP, value); }
        }
        public static readonly DependencyProperty EndPointDP =
            DependencyProperty.Register(
                "EndPoint", typeof(Point),
                typeof(UISegmentWire), null
            );
        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointDP); }
            set { SetValue(EndPointDP, value); }
        }


        public UISegmentWire()
        {
            this.InitializeComponent();
        }

        public void Update()
        {
            Bindings.Update();
        }

        public DeleteComponent OnClickDeleteSegmentWire { get; set; }
        private void MenuFlyoutItem_Remove(object sender, RoutedEventArgs e)
        {
            OnClickDeleteSegmentWire();
        }

    }
}
