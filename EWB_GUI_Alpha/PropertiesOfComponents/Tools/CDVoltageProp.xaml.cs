using EWB_GUI_Alpha.ElectronicComponents.Tools;
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

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace EWB_GUI_Alpha.PropertiesOfComponents.Tools
{
    public sealed partial class CDVoltageProp : ContentDialog
    {
        private string TEXT { get; set; }
        private UCVoltmeter voltmeter;

        public CDVoltageProp(UCVoltmeter voltmeter)
        {
            this.InitializeComponent();
            this.voltmeter = voltmeter;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Bindings.Update();
            voltmeter.IndicateValue = TEXT;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
