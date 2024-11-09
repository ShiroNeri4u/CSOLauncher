using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Diagnostics;
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
        private const string Extend = ".png";
        private const string ItemDirectory = "Item";
        private const string NonePart = "partsrandom";
        private const char BackSlash = '\\';
        private const char UnderLine = '_';
        private const string CSOItemBoxBorderName = "line";
        private const string CSOItemBoxBottomName = "bg";
        public event RoutedEventHandler Click;

        private WriteableBitmap CSOItemBoxBorder
        {
            get => (WriteableBitmap)GetValue(CSOItemBoxBorderProperty);
            set => SetValue(CSOItemBoxBorderProperty, value);
        }

        private WriteableBitmap CSOItemBoxBottom
        {
            get => (WriteableBitmap)GetValue(CSOItemBoxBottomProperty);
            set => SetValue(CSOItemBoxBottomProperty, value);
        }

        private string CSOCSOPartName
        {
            get => (string)GetValue(CSOCSOPartNameProperty);
            set => SetValue(CSOCSOPartNameProperty, value);
        }

        private object CSOPartUri
        {
            get => (object)GetValue(CSOPartUriProperty);
            set => SetValue(CSOPartUriProperty, value);
        }

        public Item CSOPartItem
        {
            get => (Item)GetValue(CSOPartItemProperty);
            set => SetValue(CSOPartItemProperty, value);
        }

        public CSOPartView()
        {
            this.InitializeComponent();
        }

        private static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
            nameof(Click),
            typeof(RoutedEventHandler),
            typeof(CSOButtonBase),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOItemBoxBorderProperty = DependencyProperty.Register(
            nameof(CSOItemBoxBorder),
            typeof(WriteableBitmap),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOItemBoxBottomProperty = DependencyProperty.Register(
            nameof(CSOItemBoxBottom),
            typeof(WriteableBitmap),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );
        private static readonly DependencyProperty CSOCSOPartNameProperty = DependencyProperty.Register(
            nameof(CSOCSOPartName),
            typeof(string),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartUriProperty = DependencyProperty.Register(
            nameof(CSOPartUri),
            typeof(object),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartItemProperty = DependencyProperty.Register(
            nameof(CSOPartItem),
            typeof(Item),
            typeof(CSOPartView),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartView;
                    self.CSOCSOPartName = self.CSOPartItem.TransName;
                    StringBuilder sb = new();
                    sb.Append(PreName);
                    sb.Append(UnderLine);
                    sb.Append(ColorString[(int)self.CSOPartItem.ItemGrade]);
                    sb.Append(UnderLine);
                    sb.Append(CSOItemBoxBorderName);
                    self.CSOItemBoxBorder = Launcher.Assets[sb.ToString()];
                    sb.Replace(CSOItemBoxBorderName, CSOItemBoxBottomName);
                    self.CSOItemBoxBottom = Launcher.Assets[sb.ToString()];
                    if (!self.CSOPartItem.IsEmpty)
                    {
                        if(Launcher.ImageResources.TryGetValue(self.CSOPartItem.ResourceName, out BitmapImage bitmap))
                        {
                            self.CSOPartUri = bitmap;
                        }
                        else
                        {
                            sb.Clear();
                            sb.Append(Launcher.ResourceDirectory);
                            sb.Append(BackSlash);
                            sb.Append(ItemDirectory);
                            sb.Append(BackSlash);
                            sb.Append(self.CSOPartItem.ResourceName.ToLower());
                            sb.Append(Extend);
                            Debug.WriteLine(sb.ToString());
                            BitmapImage newbitmap = new(new Uri(sb.ToString()));
                            Launcher.ImageResources.Add(self.CSOPartItem.ResourceName, newbitmap);
                            self.CSOPartUri = newbitmap;
                        }
                    }
                    else
                    {
                        self.CSOPartUri = Launcher.Assets[NonePart];
                    }
                }
            )
        );

        private void OnPointerPressed(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
