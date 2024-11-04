using CSODataCore;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;

namespace CSOLauncher
{
    public sealed partial class CSOFlyoutBase : UserControl
    {
        private const string PreName = "pulldown";
        private const string PostName = "@n";
        private const string Underline = "_";
        private const string At = "@";
        private static readonly string[] Position = ["bottom_center", "bottom_left", "bottom_right", "center_center", "center_left", "center_right", "top_center", "top_left", "top_right"];
        private static readonly string[] Color = ["grey", "green", "blue", "red", "gold", "purple", "redbrown"];
        private static readonly Dictionary<string, Task> ConverntTask = [];
        private const int SingleLine = 19;
        private const int Pixelbyte = 4;
        public Flyout Flyout;
        public Item Item;
        public bool GradeDefault;
        public int FlyoutWidth;
        public int FlyoutHeight;
        
        public CSOFlyoutBase()
        {
            InitializeComponent();
        }

        public void OnLoad()
        {
            GetAssets();
        }
        private void GetAssets()
        {
            int width = FlyoutWidth;
            int height = FlyoutHeight;
            string color;
            if (Item.ItemGrade == ItemGrade.None)
            {
                color = Color[(int)Item.ItemGrade];
            }
            else
            {
                color = Color[(int)Item.ItemGrade - 1];
            }
            string taskname = PreName + Underline + color + At + width + Underline + height;
            if (!Launcher.Assets.TryGetValue(taskname, out _))
            {
                if (!ConverntTask.TryGetValue(taskname, out _))
                {
                    ConverntTask.Add(taskname, Convernt(width, height, color));
                }
                Task task = ConverntTask[taskname];
                task.Wait();
            }
            Style CSOFlyoutPresenterStyle = (Style)Application.Current.Resources["CSOFlyoutPresenterStyle"];
            ImageBrush backgroundBrush = new ImageBrush
            {
                ImageSource = Launcher.Assets[taskname],
                Stretch = Stretch.None
            };
            Setter setBackground = new Setter(FlyoutPresenter.BackgroundProperty, backgroundBrush);
            CSOFlyoutPresenterStyle.Setters.Add(setBackground);
            Flyout = new Flyout()
            {
                AllowFocusOnInteraction = false,
                AllowFocusWhenDisabled = false,
                AreOpenCloseAnimationsEnabled = false,
                LightDismissOverlayMode = LightDismissOverlayMode.Off,
                Placement = FlyoutPlacementMode.Right,
                ShowMode = FlyoutShowMode.Transient,
                FlyoutPresenterStyle = CSOFlyoutPresenterStyle
            };
        }

            private static Task Convernt(int width, int height, string color)
        {
            WriteableBitmap bottom_center_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[0] + PostName];
            WriteableBitmap bottom_left_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[1] + PostName];
            WriteableBitmap bottom_right_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[2] + PostName];
            WriteableBitmap center_center_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[3] + PostName];
            WriteableBitmap center_left_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[4] + PostName];
            WriteableBitmap center_right_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[5] + PostName];
            WriteableBitmap top_center_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[6] + PostName];
            WriteableBitmap top_left_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[7] + PostName];
            WriteableBitmap top_right_bitmap = Launcher.Assets[PreName + Underline + color + Underline + Position[8] + PostName];
            byte[] bottom_center = bottom_center_bitmap.PixelBuffer.ToArray();
            byte[] bottom_left = bottom_left_bitmap.PixelBuffer.ToArray();
            byte[] bottom_right = bottom_right_bitmap.PixelBuffer.ToArray();
            byte[] center_center = center_center_bitmap.PixelBuffer.ToArray();
            byte[] center_left = center_left_bitmap.PixelBuffer.ToArray();
            byte[] center_right = center_right_bitmap.PixelBuffer.ToArray();
            byte[] top_center = top_center_bitmap.PixelBuffer.ToArray();
            byte[] top_left = top_left_bitmap.PixelBuffer.ToArray();
            byte[] top_right = top_right_bitmap.PixelBuffer.ToArray();
            int repeatwidth = width - 2 * SingleLine;
            int repeathight = height - 2 * SingleLine;
            int bottom_center_ptr = 0;
            int bottom_left_ptr = 0;
            int bottom_right_ptr = 0;
            int top_center_ptr = 0;
            int top_left_ptr = 0;
            int top_right_ptr = 0;

            List<Byte> buffer = [];
            // 绘制上部分
            for (int h = 0; h < SingleLine; h++)
            {
                // 绘制左部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(top_left[w + top_left_ptr]);
                }
                top_left_ptr += SingleLine * Pixelbyte;
                // 绘制中间重复部分
                for (int w = 0; w < repeatwidth; w++)
                {
                    for (int i = 0; i < Pixelbyte; i++)
                    {
                        buffer.Add(top_center[top_center_ptr + i]);
                    }
                }
                top_center_ptr += Pixelbyte;
                // 绘制右部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(top_right[w + top_right_ptr]);
                }
                top_right_ptr += SingleLine * Pixelbyte;
            }
            // 绘制中部重复部分
            for (int h = 0; h < repeathight; h++)
            {
                //绘制左部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(center_left[w]);
                }
                //绘制中间重复部分
                for (int w = 0; w < repeatwidth; w++)
                {
                    for (int i = 0; i < Pixelbyte; i++)
                    {
                        buffer.Add(center_center[i]);
                    }
                }
                // 绘制右部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(center_right[w]);
                }
            }
            // 绘制下部分
            for (int h = 0; h < SingleLine; h++)
            {
                // 绘制左部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(bottom_left[w + bottom_left_ptr]);
                }
                bottom_left_ptr += SingleLine * Pixelbyte;
                // 绘制中间重复部分
                for (int w = 0; w < repeatwidth; w++)
                {
                    for (int i = 0; i < Pixelbyte; i++)
                    {
                        buffer.Add(bottom_center[bottom_center_ptr + i]);
                    }
                }
                bottom_center_ptr += Pixelbyte;
                // 绘制右部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(bottom_right[w + bottom_right_ptr]);
                }
                bottom_right_ptr += SingleLine * Pixelbyte;
            }
            byte[] bytes = [.. buffer];
            WriteableBitmap bitmap = new(width, height);
            using (Stream pixelstream = bitmap.PixelBuffer.AsStream())
            {
                pixelstream.Seek(0, SeekOrigin.Begin);
                pixelstream.Write(bytes, 0, bytes.Length);
            }
            Launcher.Assets.Add(PreName + Underline + color + At + width + Underline + height, bitmap);
            return Task.CompletedTask;
        }
    }
}
