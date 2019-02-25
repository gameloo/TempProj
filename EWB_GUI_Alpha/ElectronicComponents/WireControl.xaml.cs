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

namespace EWB_GUI_Alpha.ElectronicComponents
{
    public sealed partial class WireControl : UserControl
    {
        private double X1;
        private double X2;
        private double Y1;
        private double Y2;

        private ConnectorControl connector_1;
        private ConnectorControl connector_2;

        public WireControl()
        {
            this.InitializeComponent();
        }

        public WireControl(ConnectorControl connector_1, ConnectorControl connector_2)
        {
            this.InitializeComponent();

            X1 = connector_1.PositionOnCanvas.X;
            Y1 = connector_1.PositionOnCanvas.Y;
            X2 = connector_2.PositionOnCanvas.X;
            Y2 = connector_2.PositionOnCanvas.Y;

            this.connector_1 = connector_1;
            this.connector_2 = connector_2;

            this.connector_1.update += Update;
            this.connector_2.update += Update;
        }

        private void Update()
        {
            X1 = connector_1.PositionOnCanvas.X;
            Y1 = connector_1.PositionOnCanvas.Y;
            X2 = connector_2.PositionOnCanvas.X;
            Y2 = connector_2.PositionOnCanvas.Y;
            Bindings.Update();
        }


    }
}
