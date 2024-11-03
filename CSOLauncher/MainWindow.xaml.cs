using CSODataCore;
using Microsoft.UI.Xaml;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace CSOLauncher
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        { 
            ItemManager.ImportItem(@"D:\CSNZ\Server\Data\item.csv");
            ItemManager.ImportLanguage(@"D:\CSNZ\Server\Data\cso_na_en.txt");
            ItemManager.ImportReinforce(@"D:\CSNZ\Server\Data\ReinforceMaxLv.csv");
            ItemManager.LoadLanguage();
            this.InitializeComponent();
            Border.Source = Launcher.Assets["itembox_gold"];
            Weapon.Source = Launcher.Assets["ak47"];
            Team.Source = Launcher.Assets["teamtype0"];
            Part.Source = Launcher.Assets["partsweapon_icon"];
            Part1.Source = Launcher.Assets["empty_slot"];
            Part2.Source = Launcher.Assets["empty_slot"];
            Enchant.Source = Launcher.Assets["enchant"];
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            Team.Source = Launcher.Assets["btn_default@n_50"];
        }

        private void MyButton2_Click(object sender, RoutedEventArgs e)
        {
            Item[] items = ItemManager.Search(int.Parse(Search.Text));
            Context.Text = items[0].TransName + "√Ë ˆ";
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
                LangFile.Text = filePath;
            }
        }

    }
}
