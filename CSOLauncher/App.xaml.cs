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
using Windows.Storage.Search;
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

        private static async Task LoadAssets()
        {
            StorageFolder InstalledLocation = Package.Current.InstalledLocation;
            StorageFolder AssetsFolder = await InstalledLocation.GetFolderAsync("Assets");
            StorageFolder CSOFolder = await AssetsFolder.GetFolderAsync("CSO");
            StorageFolder CommonFolder = await CSOFolder.GetFolderAsync("Common");
            StorageFolder LocalizationFolder = await CSOFolder.GetFolderAsync("zh-cn");
            List<Task> tasks = [];
            tasks.Add(FindTgaUri(CommonFolder, InstalledLocation.Path));
            tasks.Add(FindTgaUri(LocalizationFolder, InstalledLocation.Path));
            await Task.WhenAll(tasks);
        }

        private async static Task FindTgaUri(StorageFolder folder, string basePath)
        {
            List<Task> tasks = [];
            var files = await folder.GetFilesAsync(CommonFileQuery.OrderByName);
            foreach (var file in files)
            {
                if (file.FileType.Equals(".tga", StringComparison.OrdinalIgnoreCase))
                {
                    string relativePath = file.Path.Replace(basePath, "").TrimStart(System.IO.Path.DirectorySeparatorChar);
                    Uri fileUri = new($"ms-appx:///{relativePath}");
                    Task task = LoadTgaImageAsync(fileUri);
                    tasks.Add(task);
                }
            }
            await Task.WhenAll(tasks);
        }

        private async static Task LoadTgaImageAsync(Uri uri)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            
            using IRandomAccessStream stream = await file.OpenReadAsync();
            using MemoryStream memstream = new();
            using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(Options, stream.AsStreamForRead());
            {
                image.SaveAsPng(memstream);
                WriteableBitmap bitmap = new(image.Width, image.Height);
                memstream.Seek(0, SeekOrigin.Begin);
                bitmap.SetSource(memstream.AsRandomAccessStream());
                Assets.Add(System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath), bitmap);
            }
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Task task = LoadAssets();
            await task;
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
        public static Window FileView = new ();
    }
}
