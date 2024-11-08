using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CSOLauncher
{
    public sealed partial class CSOPartFlyout : UserControl
    {
        private const string NewLine = "\n";
        private bool LoadColor = false;
        private bool LoadWidth = false;
        private bool LoadHeight = false;
        private bool LoadItem = false;
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }

        public int FlyoutWidth
        {
            get => (int)GetValue(FlyoutWidthProperty);
            set => SetValue(FlyoutWidthProperty, value);
        }

        private int FlyoutHeight
        {
            get => (int)GetValue(FlyoutHeightProperty);
            set => SetValue(FlyoutHeightProperty, value);
        }

        public CSOFlyoutBase.Color CurrentColor
        {
            get => (CSOFlyoutBase.Color)GetValue(FlyoutColorProperty);
            set => SetValue(FlyoutColorProperty, value);
        }

        private WriteableBitmap CurrentBackground
        {
            get => (WriteableBitmap)GetValue(CurrentBackgroundProperty);
            set => SetValue(CurrentBackgroundProperty, value);
        }

        private string PartName
        {
            get => (string)GetValue(PartNameProperty);
            set => SetValue(PartNameProperty, value);
        }

        private string PartDesc
        {
            get => (string)GetValue(PartDescProperty);
            set => SetValue(PartDescProperty, value);
        }

        public Item PartItem
        {
            get => (Item)GetValue(PartItemProperty);
            set => SetValue(PartItemProperty, value);
        }

        private Visibility IsEmpty
        {
            get => (Visibility)GetValue(IsEmptyProperty);
            set => SetValue(IsEmptyProperty, value);
        }

        public CSOPartFlyout()
        {
            InitializeComponent();
        }

        private void OnLoad()
        {
            Set(CSOFlyoutBase.GetAssets(FlyoutWidth,FlyoutHeight,CurrentColor));
        }

        private void Set(string taskname)
        {
            CurrentBackground = Launcher.Assets[taskname];
        }

        private static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen),
            typeof(double),
            typeof(CSOPartFlyout),
            new PropertyMetadata(false)
        );

        private static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            nameof(HorizontalOffset),
            typeof(double),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            nameof(VerticalOffset),
            typeof(double),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty FlyoutWidthProperty = DependencyProperty.Register(
            nameof(FlyoutWidth),
            typeof(int),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartFlyout;
                    self.LoadWidth = true;
                    if (self.LoadWidth && self.LoadHeight && self.LoadColor && self.LoadItem)
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        private static readonly DependencyProperty FlyoutHeightProperty = DependencyProperty.Register(
            nameof(FlyoutHeight),
            typeof(int),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartFlyout;
                    self.LoadHeight = true;
                    if (self.LoadWidth && self.LoadHeight && self.LoadColor && self.LoadItem)
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        private static readonly DependencyProperty FlyoutColorProperty = DependencyProperty.Register(
            nameof(CurrentColor),
            typeof(CSOFlyoutBase.Color),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartFlyout;
                    self.LoadColor = true;
                    if (self.LoadWidth && self.LoadHeight && self.LoadColor && self.LoadItem)
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        private static readonly DependencyProperty CurrentBackgroundProperty = DependencyProperty.Register(
            nameof(CurrentBackground),
            typeof(WriteableBitmap),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty PartNameProperty = DependencyProperty.Register(
            nameof(PartName),
            typeof(string),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty PartDescProperty = DependencyProperty.Register(
            nameof(PartDesc),
            typeof(string),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty PartItemProperty = DependencyProperty.Register(
            nameof(PartItem),
            typeof(Item),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartFlyout;
                    Item item = e.NewValue as Item;
                    string text;
                    if (item.Name != null)
                    {
                        self.PartName = item.TransName;
                        StringBuilder desc = new();
                        desc.Append(ItemManager.LanguageDictionary["CSO_Item_Desc_" + item.ResourceName]);
                        desc.Replace(@"\n", NewLine);
                        text = desc.ToString();
                        self.PartDesc = text;
                    }
                    else
                    {
                        text = ItemManager.LanguageDictionary["CSO_WeaponParts_Tooltip_Empty"];
                        self.PartDesc = text;
                    }
                    TextBlock textBlock = new()
                    {
                        Text = text,
                        Width = 221,
                        FontSize = 12,
                        CharacterSpacing = 50,
                        TextWrapping = TextWrapping.WrapWholeWords,
                        FontFamily = (FontFamily)Microsoft.UI.Xaml.Application.Current.Resources["CSOFont"]
                    };
                    textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    if (item.Name != null)
                    {
                        self.IsEmpty = Visibility.Visible;
                        self.FlyoutHeight = (int)textBlock.DesiredSize.Height + 57;
                    }
                    else
                    {
                        self.IsEmpty = Visibility.Collapsed;
                        self.FlyoutHeight = (int)textBlock.DesiredSize.Height + 27;
                    }
                    self.LoadItem = true;
                    if (self.LoadWidth && self.LoadHeight && self.LoadColor && self.LoadItem)
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        private static readonly DependencyProperty IsEmptyProperty = DependencyProperty.Register(
            nameof(IsEmpty),
            typeof(Visibility),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );
    }
}
