using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CSOLauncher
{
    public class CSOItemBackgroundBase
    {
        private const string PreName = "button_list";
        private const string PostName = "@n";
        private const char Underline = '_';
        private const char At = '@';
        private static readonly string[] Position = ["top_left", "top_center", "top_right", "center_left", "center_center", "center_right", "bottom_left", "bottom_center", "bottom_right"];
        private static readonly string[] ColorString = ["c", "b", "a", "s", "ss", "sss", "rb"];
        private static readonly Dictionary<string, Task> ConverntTask = [];
        private const int SingleLine = 5;
        private const int Pixelbyte = 4;
        private const int MinBackgroundSize = 10;
        public enum Color : byte
        {
            Grey = 0,
            Green = 1,
            Blue = 2,
            Red = 3,
            Gold = 4,
            Purple = 5,
            RedBrown = 6,
        }

        public static string GetAssets(int FlyoutWidth, int FlyoutHeight, Color CurrentColor)
        {
            int width = FlyoutWidth >= MinBackgroundSize ? FlyoutWidth : MinBackgroundSize;
            int height = FlyoutHeight >= MinBackgroundSize ? FlyoutHeight : MinBackgroundSize;
            string color = ColorString[(int)CurrentColor];
            StringBuilder tasknamebuilder = new();
            tasknamebuilder.Append(PreName).Append(Underline).Append(color).Append(At).Append(width).Append(Underline).Append(height);
            string taskname = tasknamebuilder.ToString();
            if (!Launcher.Assets.TryGetValue(taskname, out _))
            {
                if (!ConverntTask.TryGetValue(taskname, out _))
                {
                    Task task = Convernt(width, height, color, taskname);
                    ConverntTask.Add(taskname, task);
                    task.Wait();
                    ConverntTask.Remove(taskname);
                }
                else
                {
                    Task task = ConverntTask[taskname];
                    task.Wait();
                }
            }
            return taskname;
        }

        private static Task Convernt(int width, int height, string color, string taskname)
        {
            StringBuilder bitmapname = new();
            List<byte[]> bytes = [];
            bitmapname.Append(PreName).Append(Underline).Append(color).Append(Underline).Append(Position[0]).Append(PostName);
            for (int i = 0; i < 9; i++)
            {
                WriteableBitmap _bitmap = Launcher.Assets[bitmapname.ToString()];
                bytes.Add(_bitmap.PixelBuffer.ToArray());
                if (i != 8)
                {
                    bitmapname.Replace(Position[i], Position[i + 1]);
                }
            }
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
                    buffer.Add(bytes[0][w + top_left_ptr]);
                }
                // 绘制中间重复部分
                for (int w = 0; w < repeatwidth; w++)
                {
                    for (int i = 0; i < Pixelbyte; i++)
                    {
                        buffer.Add(bytes[1][i + top_center_ptr]);
                    }
                }
                // 绘制右部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(bytes[2][w + top_right_ptr]);
                }
                // 偏移量
                top_left_ptr += SingleLine * Pixelbyte;
                top_center_ptr += Pixelbyte;
                top_right_ptr += SingleLine * Pixelbyte;
            }
            // 绘制中部重复部分
            for (int h = 0; h < repeathight; h++)
            {
                // 绘制左部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(bytes[3][w]);
                }
                // 绘制中间重复部分
                for (int w = 0; w < repeatwidth; w++)
                {
                    for (int i = 0; i < Pixelbyte; i++)
                    {
                        buffer.Add(bytes[4][i]);
                    }
                }
                // 绘制右部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(bytes[5][w]);
                }
            }
            // 绘制下部分
            for (int h = 0; h < SingleLine; h++)
            {
                // 绘制左部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(bytes[6][w + bottom_left_ptr]);
                }
                // 绘制中间重复部分
                for (int w = 0; w < repeatwidth; w++)
                {
                    for (int i = 0; i < Pixelbyte; i++)
                    {
                        buffer.Add(bytes[7][bottom_center_ptr + i]);
                    }
                }
                // 绘制右部分
                for (int w = 0; w < SingleLine * Pixelbyte; w++)
                {
                    buffer.Add(bytes[8][w + bottom_right_ptr]);
                }
                // 偏移量
                bottom_left_ptr += SingleLine * Pixelbyte;
                bottom_center_ptr += Pixelbyte;
                bottom_right_ptr += SingleLine * Pixelbyte;
            }
            byte[] bitmapbytes = [.. buffer];
            WriteableBitmap bitmap = new(width, height);
            using (Stream pixelstream = bitmap.PixelBuffer.AsStream())
            {
                pixelstream.Seek(0, SeekOrigin.Begin);
                pixelstream.Write(bitmapbytes, 0, bitmapbytes.Length);
            }
            Launcher.Assets.Add(taskname, bitmap);
            return Task.CompletedTask;
        }
    }
}
