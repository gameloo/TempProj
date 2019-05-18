using EWB_GUI_Alpha.ElectronicComponents;
using EWB_GUI_Alpha.ElectronicComponents.Active;
using EWB_GUI_Alpha.ElectronicComponents.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
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



        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var control = new EMFsourceControl();
            cWorkSpace.Children.Add(control);
            Canvas.SetLeft(control, CursorPosition.X);
            Canvas.SetTop(control, CursorPosition.Y);
            repeatElementType = typeof(EMFsourceControl);
        }

        private void RepeatAddingLastElement(object sender, RoutedEventArgs e)
        {
            if (repeatElementType != null)
            {
                UIElement tempUIElement;
                if (repeatElementType.Equals(typeof(ResistorControl)))
                {
                    tempUIElement = new ResistorControl();
                }
                else throw new Exception();
                cWorkSpace.Children.Add(tempUIElement);
                Canvas.SetLeft(tempUIElement, CursorPosition.X);
                Canvas.SetTop(tempUIElement, CursorPosition.Y);
            }
        }


        private void AppBarButton_Play(object sender, RoutedEventArgs e)
        {
            StopBtn.Visibility = Visibility.Visible;
            PlayBtn.Visibility = Visibility.Collapsed;

            PauseBtn.IsEnabled = true;
        }


        private void AppBarButton_Pause(object sender, RoutedEventArgs e)
        {
            PlayBtn.Visibility = Visibility.Visible;
            StopBtn.Visibility = Visibility.Collapsed;
            PauseBtn.IsEnabled = false;           
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            PlayBtn.Visibility = Visibility.Visible;
            StopBtn.Visibility = Visibility.Collapsed;
            PauseBtn.IsEnabled = false;
        }

        private async void AppBarButton_SaveAs(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker();
            // место для сохранения по умолчанию
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // устанавливаем типы файлов для сохранения
            savePicker.FileTypeChoices.Add("", new List<string>() { ".shm" });
            // устанавливаем имя нового файла по умолчанию
            savePicker.SuggestedFileName = "New snheme";
            savePicker.CommitButtonText = "Сохранить";
            var new_file = await savePicker.PickSaveFileAsync();
        }

        private async void AppBarButton_Open(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.CommitButtonText = "Открыть";
            openPicker.FileTypeFilter.Add(".shm");
            var file = await openPicker.PickSingleFileAsync();
        }

        private void AddNewConnector(object sender, RoutedEventArgs e)
        {
            var tempConnector = new ConnectorControl() { PositionOnElement = Position.center, ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY };
            cWorkSpace.Children.Add(tempConnector);
            tempConnector.AddMenuFlyout();
            Canvas.SetLeft(tempConnector, CursorPosition.X);
            Canvas.SetTop(tempConnector, CursorPosition.Y);
        }

        private void AddVoltageCurrentSource(object sender, RoutedEventArgs e)
        {
            var control = new UCVoltageCurrentSource();
            cWorkSpace.Children.Add(control);
            Canvas.SetLeft(control, CursorPosition.X);
            Canvas.SetTop(control, CursorPosition.Y);
            repeatElementType = typeof(UCVoltageCurrentSource);
        }

        private void AddAmpermetr(object sender, RoutedEventArgs e)
        {

        }

        private void AddVoltmeter(object sender, RoutedEventArgs e)
        {
            var control = new UCVoltmeter();
            cWorkSpace.Children.Add(control);
            Canvas.SetLeft(control, CursorPosition.X);
            Canvas.SetTop(control, CursorPosition.Y);
            repeatElementType = typeof(UCVoltmeter);
        }


        private void AddResistor(object sender, RoutedEventArgs e)
        {
            var tempResistor = new ResistorControl();
            cWorkSpace.Children.Add(tempResistor);
            Canvas.SetLeft(tempResistor, CursorPosition.X);
            Canvas.SetTop(tempResistor, CursorPosition.Y);
            repeatElementType = typeof(ResistorControl);
        }

        private void AddCapacitor(object sender, RoutedEventArgs e)
        {
            var control = new CapacitorControl();
            cWorkSpace.Children.Add(control);
            Canvas.SetLeft(control, CursorPosition.X);
            Canvas.SetTop(control, CursorPosition.Y);
            repeatElementType = typeof(CapacitorControl);
        }

        private void AddInductor(object sender, RoutedEventArgs e)
        {
            var control = new InductorControl();
            cWorkSpace.Children.Add(control);
            Canvas.SetLeft(control, CursorPosition.X);
            Canvas.SetTop(control, CursorPosition.Y);
            repeatElementType = typeof(InductorControl);
        }
    }
}
