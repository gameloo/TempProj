using EWB_GUI_Alpha.ElectronicComponents;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace EWB_GUI_Alpha
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Type repeatElementType;

        private Point CursorPosition { get; set; }
        void OnPressed(object sender, PointerRoutedEventArgs e)
        {
            CursorPosition = e.GetCurrentPoint(CustomVisualTreeHelper.KernelCanvas).Position;
        }
        public MainPage()
        {
            this.InitializeComponent();
            CustomVisualTreeHelper.KernelCanvas = cWorkSpace;
            this.PointerPressed += new PointerEventHandler(OnPressed);
        }


        private void AddResistor(object sender, RoutedEventArgs e)
        {
            cWorkSpace.Children.Add(new ResistorControl());
            repeatElementType = typeof(ResistorControl);
        }

        private void RepeatAddingLastElement(object sender, RoutedEventArgs e)
        {
            if (repeatElementType != null)
            {
                if (repeatElementType.Equals(typeof(ResistorControl)))
                    cWorkSpace.Children.Add(new ResistorControl());
                else throw new Exception();
            }
        }

        private void AddNewConnector(object sender, RoutedEventArgs e)
        {
            var tempConnector = new ConnectorControl() { PositionOnElement = Position.center, ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY };
            cWorkSpace.Children.Add(tempConnector);
            tempConnector.AddMenuFlyout();
            Canvas.SetLeft(tempConnector, CursorPosition.X);
            Canvas.SetTop(tempConnector, CursorPosition.Y);

        }
    }
}
