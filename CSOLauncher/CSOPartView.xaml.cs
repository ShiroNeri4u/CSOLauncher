using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace CSOLauncher
{
    public sealed partial class CSOPartView : UserControl
    {
        private static readonly string[] ColorString = ["c", "b", "a", "s", "ss", "sss", "rb"];
        private const string PreName = "itembox_random";
        private const char UnderLine = '_';
        private const string ItemBorderName = "line";
        private const string ItemBackgroundName = "bg";
        public event RoutedEventHandler Click;

        private WriteableBitmap ItemBorder
        {
            get => (WriteableBitmap)GetValue(ItemBorderProperty);
            set => SetValue(ItemBorderProperty, value);
        }
        private WriteableBitmap ItemBackground
        {
            get => (WriteableBitmap)GetValue(ItemBackgroundProperty);
            set => SetValue(ItemBackgroundProperty, value);
        }
        private string PartName
        {
            get => (string)GetValue(PartNameProperty);
            set => SetValue(PartNameProperty, value);
        }
        private object PartUri
        {
            get => (object)GetValue(PartUriProperty);
            set => SetValue(PartUriProperty, value);
        }
        public Item PartItem
        {
            get => (Item)GetValue(PartItemProperty);
            set => SetValue(PartItemProperty, value);
        }
        public CSOPartView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
            nameof(Click),
            typeof(RoutedEventHandler),
            typeof(CSOButtonBase),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty ItemBorderProperty = DependencyProperty.Register(
            nameof(ItemBorder),
            typeof(WriteableBitmap),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty ItemBackgroundProperty = DependencyProperty.Register(
            nameof(ItemBackground),
            typeof(WriteableBitmap),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );
        private static readonly DependencyProperty PartNameProperty = DependencyProperty.Register(
            nameof(PartName),
            typeof(string),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty PartUriProperty = DependencyProperty.Register(
            nameof(PartUri),
            typeof(object),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty PartItemProperty = DependencyProperty.Register(
            nameof(PartItem),
            typeof(Item),
            typeof(CSOPartView),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartView;
                    self.PartName = self.PartItem.TransName;
                    StringBuilder sb = new();
                    sb.Append(PreName);
                    sb.Append(UnderLine);
                    sb.Append(ColorString[(int)self.PartItem.ItemGrade]);
                    sb.Append(UnderLine);
                    sb.Append(ItemBackgroundName);
                    self.ItemBackground = Launcher.Assets[sb.ToString()];
                    sb.Replace(ItemBackgroundName, ItemBorderName);
                    self.ItemBorder = Launcher.Assets[sb.ToString()];
                    if (!self.PartItem.IsEmpty)
                    {
                        if(Launcher.ImageResources.TryGetValue(self.PartItem.ResourceName, out BitmapImage bitmapimage))
                        {
                            self.PartUri = bitmapimage;
                        }
                        else
                        {
                            BitmapImage bitmap = new BitmapImage(new Uri(@"D:\CSNZ\Item\Item\" + self.PartItem.ResourceName.ToLower() + ".png"));
                            Launcher.ImageResources.Add(self.PartItem.ResourceName, bitmap);
                            self.PartUri = bitmap;
                        }
                    }
                    else
                    {
                        self.PartUri = Launcher.Assets["partsrandom"];
                    }
                }
            )
        );

        private void OnPointerPressed(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }

    public partial class PartImageConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BitmapImage)
            {
                var self = value as BitmapImage;
                return self;
            }
            else if (value is WriteableBitmap)
            {
                var self = value as WriteableBitmap;
                return self;
            }
            else return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}
