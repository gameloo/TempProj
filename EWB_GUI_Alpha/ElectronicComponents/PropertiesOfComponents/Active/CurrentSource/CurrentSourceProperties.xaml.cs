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

namespace EWB_GUI_Alpha.ElectronicComponents.PropertiesOfComponents.Active.CurrentSource
{
    public sealed partial class CurrentSourceProperties : ContentDialog
    {
        private EMFsourceControl currentSource;

        public CurrentSourceProperties(EMFsourceControl currentSource)
        {
            this.InitializeComponent();
            this.currentSource = currentSource;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var resultString = "";
            if (Double.TryParse(tbAmperage.Text, out double result))
            {
                if (cbMultipler.SelectedIndex == 0) result *= 0.000001;
                if (cbMultipler.SelectedIndex == 1) result *= 0.001;

                if (result > 0.1) resultString = $"{result}A";
                else if (result > 0.00001) resultString = $"{result * 1000}мA";
                else if (result > 0.00001) resultString = $"{result * 1000000}\u00B5A";
                currentSource.IndicatorValue = resultString;
                currentSource.ComponentName = tbNameCompont.Text;
            }
            else args.Cancel = true;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
