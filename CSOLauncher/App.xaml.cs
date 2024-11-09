using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;

namespace CSOLauncher
{

    public partial class Launcher : Application
    {
        public Launcher()
        {
            this.InitializeComponent();
        }

        private static readonly DecoderOptions Options = new()
        {
            SkipMetadata = true,
        };

        public static readonly Dictionary<string, WriteableBitmap> Assets = [];
        public static readonly Dictionary<string, BitmapImage> ImageResources = [];
        private static readonly List<Task> LoadAssetsTask = [];
        private static readonly string[] Folders = ["Common", "zh-cn"];
        public static string ResourceDirectory = @"D:\CSNZ\Item";
        private static async Task LoadAssets()
        {
            StorageFolder InstalledLocation = Package.Current.InstalledLocation;
            StorageFolder AssetsFolder = await InstalledLocation.GetFolderAsync("Assets");
            StorageFolder CSOFolder = await AssetsFolder.GetFolderAsync("CSO");
            foreach( string folder in Folders)
            {
                StorageFolder Folder = await CSOFolder.GetFolderAsync(folder);
                LoadAssetsTask.Add(FindTgaUri(Folder, InstalledLocation.Path));
            }
            await Task.WhenAll(LoadAssetsTask);
        }

        private async static Task FindTgaUri(StorageFolder folder, string basePath)
        {
            List<Task> tasks = [];
            string[] files = Directory.GetFiles(folder.Path);
            foreach (string file in files)
            {
                string relativePath = file.Replace(basePath, "").TrimStart(System.IO.Path.DirectorySeparatorChar);
                Uri fileUri = new($"ms-appx:///{relativePath}");
                Task task = LoadTgaImageAsync(fileUri);
                tasks.Add(task);
            }
            IReadOnlyList<StorageFolder> folders = await folder.GetFoldersAsync();
            foreach (StorageFolder nextfolder in folders)
            {
                tasks.Add(FindTgaUri(nextfolder, basePath));
            }
            await Task.WhenAll(tasks);
        }

        private async static Task LoadTgaImageAsync(Uri uri)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            string resourcesname = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
            using IRandomAccessStream stream = await file.OpenReadAsync();
            using MemoryStream memstream = new();
            using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(Options, stream.AsStreamForRead());
            {
                WriteableBitmap bitmap = new(image.Width, image.Height);
                image.SaveAsPng(memstream);
                memstream.Seek(0, SeekOrigin.Begin);
                bitmap.SetSource(memstream.AsRandomAccessStream());
                Assets.Add(resourcesname, bitmap);
            }
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Task task = LoadAssets();
            await task;
            ItemManager.ImportItem(@"D:\CSNZ\Server\Data\item.csv");
            ItemManager.ImportLanguage(@"D:\CSNZ\Server\Data\cso_na_en.txt");
            ItemManager.ImportReinforce(@"D:\CSNZ\Server\Data\ReinforceMaxLv.csv");
            ItemManager.ImportPaints(@"D:\CSNZ\Server\WeaponPaints.json");
            ItemManager.LoadLanguage();
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
        public static Window FileView = new ();
    }
}
