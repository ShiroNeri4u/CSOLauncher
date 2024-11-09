using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
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
        private WriteableBitmap CSOPartFlyoutBorderBackground
        {
            get => (WriteableBitmap)GetValue(CSOPartFlyoutBorderBackgroundProperty);
            set => SetValue(CSOPartFlyoutBorderBackgroundProperty, value);
        }
        private string CSOPartName
        {
            get => (string)GetValue(CSOPartNameProperty);
            set => SetValue(CSOPartNameProperty, value);
        }

        private string CSOPartDesc
        {
            get => (string)GetValue(CSOPartDescProperty);
            set => SetValue(CSOPartDescProperty, value);
        }

        private Visibility CSOPartFlyoutIsEmpty
        {
            get => (Visibility)GetValue(CSOPartFlyoutIsEmptyProperty);
            set => SetValue(CSOPartFlyoutIsEmptyProperty, value);
        }

        public bool CSOPartFlyoutIsOpen
        {
            get => (bool)GetValue(CSOPartFlyoutIsOpenProperty);
            set => SetValue(CSOPartFlyoutIsOpenProperty, value);
        }

        public double CSOPartFlyoutHorizontalOffset
        {
            get => (double)GetValue(CSOPartFlyoutHorizontalOffsetProperty);
            set => SetValue(CSOPartFlyoutHorizontalOffsetProperty, value);
        }

        public double CSOPartFlyoutVerticalOffset
        {
            get => (double)GetValue(CSOPartFlyoutVerticalOffsetProperty);
            set => SetValue(CSOPartFlyoutVerticalOffsetProperty, value);
        }

        public int CSOPartFlyoutWidth
        {
            get => (int)GetValue(CSOPartFlyoutWidthProperty);
            set => SetValue(CSOPartFlyoutWidthProperty, value);
        }

        private int CSOPartFlyoutHeight
        {
            get => (int)GetValue(CSOPartFlyoutHeightProperty);
            set => SetValue(CSOPartFlyoutHeightProperty, value);
        }

        public CSOFlyoutBase.Color CSOPartFlyoutBorderColor
        {
            get => (CSOFlyoutBase.Color)GetValue(CSOPartFlyoutBorderColorProperty);
            set => SetValue(CSOPartFlyoutBorderColorProperty, value);
        }

        public Item CSOPartItem
        {
            get => (Item)GetValue(CSOPartItemProperty);
            set => SetValue(CSOPartItemProperty, value);
        }

        public CSOPartFlyout()
        {
            InitializeComponent();
        }

        private void OnLoad()
        {
            Set(CSOFlyoutBase.GetAssets(CSOPartFlyoutWidth,CSOPartFlyoutHeight,CSOPartFlyoutBorderColor));
        }

        private void Set(string taskname)
        {
            CSOPartFlyoutBorderBackground = Launcher.Assets[taskname];
        }

        private static readonly DependencyProperty CSOPartFlyoutBorderBackgroundProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutBorderBackground),
            typeof(WriteableBitmap),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartNameProperty = DependencyProperty.Register(
            nameof(CSOPartName),
            typeof(string),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartDescProperty = DependencyProperty.Register(
            nameof(CSOPartDesc),
            typeof(string),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartFlyoutIsEmptyProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutIsEmpty),
            typeof(Visibility),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartFlyoutIsOpenProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutIsOpen),
            typeof(double),
            typeof(CSOPartFlyout),
            new PropertyMetadata(false)
        );

        private static readonly DependencyProperty CSOPartFlyoutHorizontalOffsetProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutHorizontalOffset),
            typeof(double),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartFlyoutVerticalOffsetProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutVerticalOffset),
            typeof(double),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartFlyoutWidthProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutWidth),
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

        private static readonly DependencyProperty CSOPartFlyoutHeightProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutHeight),
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

        private static readonly DependencyProperty CSOPartFlyoutBorderColorProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutBorderColor),
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


        public static readonly DependencyProperty CSOPartItemProperty = DependencyProperty.Register(
            nameof(CSOPartItem),
            typeof(Item),
            typeof(CSOPartFlyout),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartFlyout;
                    Item item = self.CSOPartItem;
                    string text;
                    if (!item.IsEmpty)
                    {
                        self.CSOPartName = item.TransName;
                        StringBuilder desc = new();
                        desc.Append(ItemManager.LanguageDictionary["CSO_Item_Desc_" + item.ResourceName]);
                        desc.Replace(@"\n", NewLine);
                        text = desc.ToString();
                        self.CSOPartDesc = text;
                    }
                    else
                    {
                        text = ItemManager.LanguageDictionary["CSO_WeaponParts_Tooltip_Empty"];
                        self.CSOPartDesc = text;
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
                    if (!item.IsEmpty)
                    {
                        self.CSOPartFlyoutIsEmpty = Visibility.Visible;
                        self.CSOPartFlyoutHeight = (int)textBlock.DesiredSize.Height + 57;
                    }
                    else
                    {
                        self.CSOPartFlyoutIsEmpty = Visibility.Collapsed;
                        self.CSOPartFlyoutHeight = (int)textBlock.DesiredSize.Height + 27;
                    }
                    self.LoadItem = true;
                    if (self.LoadWidth && self.LoadHeight && self.LoadColor && self.LoadItem)
                    {
                        self.OnLoad();
                    }
                }
            )
        );

    }
}
