using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace CSOLauncher
{
    public sealed partial class CSOButton : UserControl
    {
        private const string Left = "left@";
        private const string Center = "center@";
        private const string Right = "right@";
        private const string ButtonPre = "btn";
        private const string Underline = "_";
        private const string At = "@";
        private const string Default = "default";
        private const string Invensort = "invensort";
        private const string InvensortOff = "invensort_off";
        private const int ButtonHeight = 32;
        private const int ButtonHeightFix = 30;
        private const int ButtonSingleWidth = 10;
        private const int ButtonSingleFixWidth = 8;
        private const int FontHeight = 20;
        private int Count;
        private readonly static Dictionary<string, Task> ConventTask = [];

        public enum Type : int
        {
            Default = 0,
            Invensort = 1,
            InvensortOff = 2,
        };

        private static readonly string[] Status = ["n", "o", "c"];
        private WriteableBitmap ButtonNormal { get; set; }
        private WriteableBitmap ButtonOn { get; set; }
        private WriteableBitmap ButtonClick { get; set; }

        public CSOButton()
        {
            this.InitializeComponent();
        }

        private async void GetAssets()
        {
            string type = (Type)ButtonType switch
            {
                Type.Default => Default,
                Type.Invensort => Invensort,
                Type.InvensortOff => InvensortOff,
                _ => Default,
            };
            List<Task> tasks = [];
            int width = Math.Clamp(ButtonWidth, 21, int.MaxValue);
            foreach (string status in Status)
            {
                string asset = ButtonPre + Underline + type + At + status + Underline + width + Underline;
                if (!Launcher.Assets.TryGetValue(asset, value: out _))
                {
                    if (ConventTask.TryGetValue(type + status + width, out Task task))
                    {
                        tasks.Add(task);
                    }
                    else
                    {
                        task = CreateButton(width, type, status);
                        ConventTask.Add(type + status + width ,task);
                        tasks.Add(task);
                    }
                }
            }
            await Task.WhenAll(tasks);
            ButtonNormal = Launcher.Assets[ButtonPre + Underline + type + At + Status[0] + Underline + width];
            ButtonOn = Launcher.Assets[ButtonPre + Underline + type + At + Status[1] + Underline + width];
            ButtonClick = Launcher.Assets[ButtonPre + Underline + type + At + Status[2] + Underline + width];
            ButtonBackground.Source = ButtonNormal;
        }

        private void TweakWidthHeghit()
        {
            int height = (Type)ButtonType switch
            {
                Type.Default => ButtonHeight,
                Type.Invensort => ButtonHeightFix,
                Type.InvensortOff => ButtonHeightFix,
                _ => ButtonHeight,
            };
            int width = Math.Clamp(ButtonWidth, 21, int.MaxValue);
            if (double.IsNaN(ButtonScale))
            {
                ButtonBackground.Width = width;
                ButtonBackground.Height = height;
                Button.Width = width;
                Button.Height = height;
                ButtonText.FontSize = FontHeight;
            }
            else
            {
                ButtonBackground.Width = width * ButtonScale;
                ButtonBackground.Height = height * ButtonScale;
                Button.Width = width * ButtonScale;
                Button.Height = height * ButtonScale;
                ButtonText.FontSize = FontHeight * ButtonScale;
            }
        }

        private static Task CreateButton(int width, string type, string status)
        {
            int height;
            int leftwidth;
            int rightwidth;
            if (type == "default")
            {
                height = ButtonHeight;
                leftwidth = ButtonSingleWidth;
                rightwidth = ButtonSingleFixWidth;
            }
            else
            {
                height = ButtonHeightFix;
                leftwidth = ButtonSingleWidth;
                rightwidth = ButtonSingleFixWidth;
            }
            WriteableBitmap bitmap = new(width, height);
            WriteableBitmap leftbitmap = Launcher.Assets[ButtonPre + Underline + type + Underline + Left + status];
            WriteableBitmap centerbitmap = Launcher.Assets[ButtonPre + Underline + type + Underline + Center + status];
            WriteableBitmap rightbitmap = Launcher.Assets[ButtonPre + Underline + type + Underline + Right + status];
            int repeatcount = width - ButtonSingleWidth - ButtonSingleFixWidth;
            byte[] left = leftbitmap.PixelBuffer.ToArray();
            byte[] center = centerbitmap.PixelBuffer.ToArray();
            byte[] right = rightbitmap.PixelBuffer.ToArray();
            List<byte> buffer = [];
            int leftpixelindex = 0;
            int centerpixelindex = 0;
            int rightpixelindex = 0;
            //每行处理
            for (int i = 0; i < height; i++)
            {
                //左边部分
                for (int l = leftpixelindex; l < leftpixelindex + 4 * leftwidth; l++)
                {
                    buffer.Add(left[l]);
                }
                leftpixelindex += 4 * leftwidth;
                //中间填充部分
                for (int c = 0; c < repeatcount; c++ )
                {
                    buffer.Add(center[centerpixelindex]);
                    buffer.Add(center[centerpixelindex + 1]);
                    buffer.Add(center[centerpixelindex + 2]);
                    buffer.Add(center[centerpixelindex + 3]);
                }
                centerpixelindex += 4;
                //右边部分(并舍弃部分)
                for (int r = rightpixelindex; r < rightpixelindex + 4 * leftwidth; r++)
                {
                    if(r < rightpixelindex + 4 * rightwidth)
                    {
                        buffer.Add(right[r]);
                    }
                }
                rightpixelindex += 4 * leftwidth;
            }
            byte[] bytes = [.. buffer];
            using (Stream pixelstream = bitmap.PixelBuffer.AsStream())
            {
                pixelstream.Seek(0, SeekOrigin.Begin);
                pixelstream.Write(bytes, 0, bytes.Length);
            }
            Launcher.Assets.Add(ButtonPre + Underline + type + At + status + Underline + width, bitmap);
            ConventTask.Remove(type + status + width);
            return Task.CompletedTask;
        }

        public event RoutedEventHandler Click;

        public string Text {  
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Brush TextColor
        {
            get => (Brush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        
        public int ButtonWidth
        {
            get => (int)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }

        public int ButtonType
        {
            get => (int)GetValue(ButtonTypeProperty);
            set => SetValue(ButtonTypeProperty, value);
        }

        public double ButtonScale
        {
            get => (double)GetValue(ButtonScaleProperty);
            set => SetValue(ButtonScaleProperty, value);
        }

        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
            nameof(Click),
            typeof(RoutedEventHandler),
            typeof(CSOButton),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(CSOButton),
            new PropertyMetadata(default(string),
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                    {
                        var self = obj as CSOButton;
                        self.ButtonText.Text = e.NewValue as string;
                    }
                )
        );

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            nameof(TextColor),
            typeof(Brush),
            typeof(CSOButton),
            new PropertyMetadata(default(string),
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOButton;
                    self.ButtonText.Foreground = e.NewValue as Brush;
                }
            )
        );

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(
            nameof(ButtonWidth),
            typeof(int),
            typeof(CSOButton),
            new PropertyMetadata(default(int),
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOButton;
                    if (self.Count < 1)
                    {
                        self.Count++;
                    }
                    self.GetAssets();
                }
            )
        );

        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register(
            nameof(ButtonType),
            typeof(int),
            typeof(CSOButton),
            new PropertyMetadata(default(int),
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOButton;
                    if (self.Count < 1)
                    {
                        self.Count++;
                    }
                    self.GetAssets();
                }
            )
        );

        public static readonly DependencyProperty ButtonScaleProperty = DependencyProperty.Register(
            nameof(ButtonScaleProperty),
            typeof(double),
            typeof(CSOButton),
            new PropertyMetadata(default(double),
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOButton;
                    self.TweakWidthHeghit();
                }
            )
        );

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ButtonBackground.Source = ButtonOn;
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ButtonBackground.Source = ButtonClick;
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ButtonBackground.Source = ButtonOn;
            Click?.Invoke(this, e);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            ButtonBackground.Source = ButtonNormal;
        }

        private void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            OnPointerExited(sender, e);
        }

        private void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            OnPointerExited(sender, e);
        }
    }
}
