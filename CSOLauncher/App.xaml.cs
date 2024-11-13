using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            SkipMetadata = false,
        };

        public static readonly Dictionary<string, WriteableBitmap> Assets = [];
        public static readonly Dictionary<string, BitmapImage> ImageResources = [];
        private static readonly List<Task> LoadAssetsTask = [];
        private static readonly string[] Folders = ["Common", "zh-cn"];
        public static readonly bool AllowReinforceIgnoreMaxLv = true;
        private static string CSOFolder = $@"{Package.Current.InstalledLocation.Path}\Assets\CSO\";
        public static string ResourceDirectory = @"D:\CSNZ\Item";
        private static async Task LoadAssets()
        {
            foreach ( string folder in Folders)
            {
                string currentfolder = CSOFolder + folder;
                LoadAssetsTask.Add(FindTgaUri(currentfolder));
            }
            await Task.WhenAll(LoadAssetsTask);
        }

        private async static Task FindTgaUri(string folder)
        {
            List<Task> tasks = [];
            DirectoryInfo directoryInfo = new(folder);
            FileInfo[] tgaFiles = directoryInfo.GetFiles("*.tga", SearchOption.AllDirectories);

            foreach (FileInfo file in tgaFiles)
            {
                string path = file.FullName;
                tasks.Add(LoadTgaImageAsync(path));
            }
            await Task.WhenAll(tasks);
        }

        private static Task LoadTgaImageAsync(string path)
        {
            string resourcesname = System.IO.Path.GetFileNameWithoutExtension(path);
            using FileStream file = new(path, FileMode.Open, FileAccess.Read);
            using MemoryStream memstream = new();
            using SixLabors.ImageSharp.Image<Bgra32> image = SixLabors.ImageSharp.Image.Load<Bgra32>(Options, file);
            {
                WriteableBitmap bitmap = new(image.Width, image.Height);
                image.SaveAsPng(memstream);
                memstream.Seek(0, SeekOrigin.Begin);
                bitmap.SetSource(memstream.AsRandomAccessStream());
                Assets.Add(resourcesname, bitmap);
            }
            return Task.CompletedTask;
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

        public delegate void ViewModelPropertyChanged();
        private Window? m_window;
        public static Window FileView = new ();
    }
}
