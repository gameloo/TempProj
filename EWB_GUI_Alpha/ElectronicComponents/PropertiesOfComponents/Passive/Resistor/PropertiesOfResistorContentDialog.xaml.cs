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

namespace EWB_GUI_Alpha.ElectronicComponents.PropertiesOfComponents.Passive.Resistor
{
    public sealed partial class PropertiesOfResistorContentDialog : ContentDialog
    {
        private ResistorControl resistor;

        public PropertiesOfResistorContentDialog(ResistorControl resistor)
        {
            this.InitializeComponent();
            this.resistor = resistor;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (Double.TryParse(tbResistace.Text, out double result))
            {
                resistor.ResistanceValue = result;
                resistor.ComponentName = tbNameCompont.Text;
            }
            else args.Cancel = true;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
