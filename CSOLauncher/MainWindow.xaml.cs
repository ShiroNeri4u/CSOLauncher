using CSODataCore;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace CSOLauncher
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            Border.Source = Launcher.Assets["itembox_gold"];
            Weapon.Source = Launcher.Assets["ak47"];
            Team.Source = Launcher.Assets["teamtype0"];
            Part.Source = Launcher.Assets["partsweapon_icon"];
            Enchant.Source = Launcher.Assets["enchant"];
            Item[] items = ItemManager.Search(1);
            Name.Text = items[0].TransName;
        }

       

        private async void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            nint hwnd = WinRT.Interop.WindowNative.GetWindowHandle(Launcher.FileView);

            FileOpenPicker picker = new()
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            picker.FileTypeFilter.Add(".txt");
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                string filePath = file.Path;
            }
        }

    }
}
