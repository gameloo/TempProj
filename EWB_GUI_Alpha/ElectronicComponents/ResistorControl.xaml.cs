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

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace EWB_GUI_Alpha.ElectronicComponents
{
    public partial class ResistorControl : UserControl
    {
        private double top;
        private double left;

        private int spRILeft = 50;
        private int spRITop = 20;

        private double angle = 0;

        public Point PositionOnCanvas
        {
            get
            {
                return new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            }
        }

        public double Angle
        {
            set
            {
                if (value == 0)
                {
                    angle = value;
                    spRILeft = 50;
                    spRITop = 20;
                }
                else if (value == 90)
                {
                    angle = value;
                    spRILeft = 90;
                    spRITop = 50;
                }
                else throw new ArgumentException();
            }
            get { return angle; }
        }

        public double ResistanceValue { get; set; }


        public ResistorControl()
        {
            this.InitializeComponent();
        }

        public ResistorControl(ResistorControl resistorControl)
        {
            this.InitializeComponent();
            this.Angle = resistorControl.Angle;
            this.ResistanceValue = resistorControl.ResistanceValue;
        }

        private void RotateElement(object sender, RoutedEventArgs e)
        {
            if (Angle == 0)
            {
                Angle = 90;
            }
            else
            {
                Angle = 0;
            }
            Bindings.Update();
        }

        private void UserControl_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var left = Canvas.GetLeft(this) + e.Delta.Translation.X;
            var top = Canvas.GetTop(this) + e.Delta.Translation.Y;
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);

            connector_1.UpdatePositionOnCanvas(); //! Test
            connector_2.UpdatePositionOnCanvas(); //! Test
        }

        private void UserControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var left = Canvas.GetLeft(this);
            var top = Canvas.GetTop(this);



            // Выравнивание элемента
            left = Math.Round(left);
            top = Math.Round(top);

            if (left % 120 < 60)
                left -= left % 120;
            else left += (120 - left % 120);

            if (top % 120 < 60)
                top -= top % 120;
            else top += (120 - top % 120);

            left = left < 0 ? 0 : left;
            top = top < 0 ? 0 : top;
            // *

            // Проверка на наличие других элементов в Canvas с такими же координатами
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            var uIElement = (parent as Panel).Children.FirstOrDefault(element =>
            {
                return ((Canvas.GetLeft(element) == left) && (Canvas.GetTop(element) == top));
            });
            if (uIElement == null)
            {
                Canvas.SetLeft(this, left);
                Canvas.SetTop(this, top);
                this.left = left;
                this.top = top;
            }
            else
            {
                Canvas.SetLeft(this, this.left);
                Canvas.SetTop(this, this.top);
            }
            // *
            connector_1.UpdatePositionOnCanvas(); //! Test
            connector_2.UpdatePositionOnCanvas(); //! Test
        }

        private void DeleteElement(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            (parent as Panel).Children.Remove(this);
        }
    }
}
