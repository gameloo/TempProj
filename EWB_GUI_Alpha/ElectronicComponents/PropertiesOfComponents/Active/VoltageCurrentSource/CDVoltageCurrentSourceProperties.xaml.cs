using EWB_GUI_Alpha.ElectronicComponents.Active;
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

namespace EWB_GUI_Alpha.ElectronicComponents.PropertiesOfComponents.Active.VoltageCurrentSource
{
    public sealed partial class CDVoltageCurrentSourceProperties : ContentDialog
    {
        private string H1Text { get; set; } = "Напряжение";
        private string[] Waveform { get; } = new string[] { "(AC) Переменный", "(DC) Постоянный" };
        private string[] FrequencyMultipler { get; } = new string[] { "Гц", "МГц","ГГц" };
        private string[] VoltageMultipler { get; } = new string[] { "мкВ", "мВ","В" };

        private readonly UCVoltageCurrentSource UCVoltageCurrentSource;

        public CDVoltageCurrentSourceProperties(UCVoltageCurrentSource voltageCurrentSource)
        {
            this.InitializeComponent();
            this.UCVoltageCurrentSource = voltageCurrentSource;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var resultString = "";
            if (Double.TryParse(tbVoltage.Text, out double result))
            {
                if (cbMultipler.SelectedIndex == 0) result *= 0.000001;
                if (cbMultipler.SelectedIndex == 1) result *= 0.001;

                if (result > 0.1) resultString = $"{result}В";
                else if (result > 0.00001) resultString = $"{result * 1000}мВ";
                else resultString = $"{result * 1000000}\u00B5В";
            }
            else args.Cancel = true;
            if (cbACDC.SelectedIndex == 0)
            {

                if (Double.TryParse(tbFrequency.Text, out double frequency))
                {

                    switch (cbMultiplerF.SelectedIndex)
                    {
                        case 0:
                            {
                                resultString += $"\u007E{frequency}Гц";
                                break;
                            }
                        case 1:
                            {
                                resultString += $"\u007E{frequency}МГц";
                                break;
                            }
                        case 2:
                            {
                                resultString += $"\u007E{frequency}ГГц";
                                break;
                            }
                    }

                }
                else args.Cancel = true;
            }
            UCVoltageCurrentSource.IndicatorValue = resultString;
            UCVoltageCurrentSource.ComponentName = tbNameCompont.Text;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
