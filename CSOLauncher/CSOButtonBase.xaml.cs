using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CSOLauncher
{
    public sealed partial class CSOButtonBase : UserControl
    {
        private const string PreName = "btn";
        private const char Underline = '_';
        private const char At = '@';
        private static readonly string[] Position = ["left", "center", "right"];
        private static readonly string[] TypeString = ["default"];
        private static readonly string[] StatusString= ["@n", "@c", "@o"];
        private static readonly Dictionary<string, Task> ConverntTask = [];
        private const int ButtonHeight = 32;
        private const int ButtonLeftWidth = 10;
        private const int ButtonRightWidth = 8;
        private const int Pixelbyte = 4;
        private const int MinButtonWidth = 18;
        private int LoadCount = 0;
        private readonly WriteableBitmap[] ButtonBackground = new WriteableBitmap[3];
        public event RoutedEventHandler? Click;

        public int ButtonWidth
        {
            get => (int)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }

        public Type CurrentType
        {
            get => (Type)GetValue(CurrentTypeProperty);
            set => SetValue(CurrentTypeProperty, value);
        }

        public double ButtonScale
        {
            get => (double)GetValue(ButtonScaleProperty);
            set => SetValue(ButtonScaleProperty, value);
        }

        public string ButtonContext
        {
            get => (string)GetValue(ButtonContextProperty);
            set => SetValue(ButtonContextProperty, value);
        }

        public Brush TextColor
        {
            get => (Brush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        private WriteableBitmap CSOPartFlyoutBorderBackground
        {
            get => (WriteableBitmap)GetValue(CSOPartFlyoutBorderBackgroundProperty);
            set => SetValue(CSOPartFlyoutBorderBackgroundProperty, value);
        }

        public enum Type : byte
        {
            Default = 0,
        };

        public enum Status : byte
        {
            Normal = 0,
            Click = 1,
            On = 2,
        }

        public CSOButtonBase()
        {
            this.InitializeComponent();
        }

        private Task OnLoad()
        {
            GetBackground();
            CSOPartFlyoutBorderBackground = ButtonBackground[(int)Status.Normal];
            return Task.CompletedTask;
        }

        private void Set(Status status)
        {
            CSOPartFlyoutBorderBackground = ButtonBackground[(int)status];
        }

        private string GetAssets(string status)
        {
            int width = ButtonWidth > MinButtonWidth ? ButtonWidth : 21 ;
            string type = TypeString[(int)CurrentType];
            StringBuilder tasknamebuilder = new();
            tasknamebuilder.Append(PreName).Append(Underline).Append(type).Append(At).Append(status).Append(Underline).Append(width);
            string taskname = tasknamebuilder.ToString();
            if (!Launcher.Assets.TryGetValue(taskname, out _))
            {
                if (!ConverntTask.TryGetValue(taskname, out _))
                {
                    Task task = Convernt(width, type, status, taskname);
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

        private void GetBackground()
        {
            for (int i = 0; i < 3; i++)
            {
                ButtonBackground[i] = Launcher.Assets[GetAssets(StatusString[i])];
            }
        }

        private static Task Convernt(int width, string type, string status, string taskname)
        {
            StringBuilder bitmapname = new();
            List<byte[]> bytes = [];
            bitmapname.Append(PreName).Append(Underline).Append(type).Append(Underline).Append(Position[0]).Append(status);
            for (int i = 0; i < 3; i++)
            {
                WriteableBitmap _bitmap = Launcher.Assets[bitmapname.ToString()];
                bytes.Add(_bitmap.PixelBuffer.ToArray());
                if (i != 2)
                {
                    bitmapname.Replace(Position[i], Position[i + 1]);
                }
            }
            int repeatcount = width - ButtonLeftWidth - ButtonRightWidth;
            int left_ptr = 0;
            int center_ptr = 0;
            int right_ptr = 0;

            List<byte> buffer = [];
            // 绘制每行
            for (int h = 0; h < ButtonHeight; h++)
            {
                // 绘制左部分
                for (int w = 0; w < Pixelbyte * ButtonLeftWidth; w++)
                {
                    buffer.Add(bytes[0][w + left_ptr]);
                }
                // 绘制中间中间重复部分
                for (int w = 0; w < repeatcount; w++)
                {
                    for (int i = 0; i < Pixelbyte; i++)
                    {
                        buffer.Add(bytes[1][i + center_ptr]);
                    }
                }
                // 绘制右边部分
                for (int w = 0; w < Pixelbyte * ButtonRightWidth; w++)
                {
                     buffer.Add(bytes[2][w + right_ptr]);
                }
                left_ptr += Pixelbyte * ButtonLeftWidth;
                center_ptr += Pixelbyte;
                right_ptr += Pixelbyte * ButtonLeftWidth;
            }
            byte[] bitmapbytes = [.. buffer];
            WriteableBitmap bitmap = new(width, ButtonHeight);
            using (Stream pixelstream = bitmap.PixelBuffer.AsStream())
            {
                pixelstream.Seek(0, SeekOrigin.Begin);
                pixelstream.Write(bitmapbytes, 0, bitmapbytes.Length);
            }
            Launcher.Assets.Add(taskname, bitmap);
            return Task.CompletedTask;
        }

        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
            nameof(Click),
            typeof(RoutedEventHandler),
            typeof(CSOButtonBase),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty CurrentTypeProperty = DependencyProperty.Register(
            nameof(CurrentType),
            typeof(Type),
            typeof(CSOButtonBase),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOButtonBase;
                    if (self.LoadCount < 1)
                    {
                        self.LoadCount++;
                    }
                    else
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register(
            nameof(ButtonWidth),
            typeof(int),
            typeof(CSOButtonBase),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOButtonBase;
                    if (self.LoadCount < 1)
                    {
                        self.LoadCount++;
                    }
                    else
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        public static readonly DependencyProperty ButtonScaleProperty = DependencyProperty.Register(
            nameof(ButtonScale),
            typeof(double),
            typeof(CSOButtonBase),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty ButtonContextProperty = DependencyProperty.Register(
            nameof(ButtonContext),
            typeof(string),
            typeof(CSOButtonBase),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            nameof(TextColor),
            typeof(Brush),
            typeof(CSOButtonBase),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty CSOPartFlyoutBorderBackgroundProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutBorderBackground),
            typeof(WriteableBitmap),
            typeof(CSOButtonBase),
            new PropertyMetadata(null)
        );

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Set(Status.On);
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Set(Status.Click);
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Set(Status.On);
            Click?.Invoke(this, e);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Set(CSOButtonBase.Status.Normal);
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
