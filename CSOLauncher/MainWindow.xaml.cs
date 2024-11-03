using CSODataCore;
using Microsoft.UI.Xaml;
using System;
using System.Linq;
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
            ItemManager.ImportPaints(@"D:\CSNZ\Server\WeaponPaints.json");
            ItemManager.LoadLanguage();
            this.InitializeComponent();
            Border.Source = Launcher.Assets["itembox_gold"];
            Weapon.Source = Launcher.Assets["ak47"];
            Team.Source = Launcher.Assets["teamtype0"];
            Part.Source = Launcher.Assets["partsweapon_icon"];
            Enchant.Source = Launcher.Assets["enchant"];
        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            Item[] items = ItemManager.Search(int.Parse(Seachitem.Text));
            Name.Text = items[0].WeaponInfomation.Paints.Length.ToString();
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
