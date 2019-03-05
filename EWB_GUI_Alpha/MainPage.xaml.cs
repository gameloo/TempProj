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

        public MainPage()
        {
            this.InitializeComponent();
            CustomVisualTreeHelper.KernelCanvas = cWorkSpace;
        }


        private void AddResistor(object sender, RoutedEventArgs e)
        {
            //Control.PointerMovedEvent;
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
    }
}
