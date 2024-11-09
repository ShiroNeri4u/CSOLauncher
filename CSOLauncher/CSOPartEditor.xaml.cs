using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSOLauncher
{

    public sealed partial class CSOPartEditor : UserControl
    {
        private bool LoadColor = false;
        private bool LoadWidth = false;
        private bool LoadHeight = false;
        private bool LoadItem = false;
        public Item CurrentPart
        {
            get => (Item)GetValue(CurrentPartProperty);
            set => SetValue(CurrentPartProperty, value);
        }

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

        public int FlyoutHeight
        {
            get => (int)GetValue(FlyoutHeightProperty);
            set => SetValue(FlyoutHeightProperty, value);
        }

        public CSOFlyoutBase.Color CurrentColor
        {
            get => (CSOFlyoutBase.Color)GetValue(FlyoutColorProperty);
            set => SetValue(FlyoutColorProperty, value);
        }

        private double ScrollViewerWidth
        {
            get => (double)GetValue(ScrollViewerWidthProperty);
            set => SetValue(ScrollViewerWidthProperty, value);
        }

        private double ScrollViewerHeight
        {
            get => (double)GetValue(ScrollViewerHeightProperty);
            set => SetValue(ScrollViewerHeightProperty, value);
        }
        private WriteableBitmap CurrentBackground
        {
            get => (WriteableBitmap)GetValue(CurrentBackgroundProperty);
            set => SetValue(CurrentBackgroundProperty, value);
        }
        public ObservableCollection<Item> PartItems
        {
            get => (ObservableCollection<Item>)GetValue(PartItemsProperty);
            set => SetValue(PartItemsProperty, value);
        }

        public CSOPartEditor()
        {
            this.InitializeComponent();
        }

        private void OnLoad()
        {
            Set(CSOFlyoutBase.GetAssets(FlyoutWidth, FlyoutHeight, CurrentColor));
        }

        private void Set(string taskname)
        {
            CurrentBackground = Launcher.Assets[taskname];
        }

        public static readonly DependencyProperty CurrentPartProperty = DependencyProperty.Register(
            nameof(CurrentPart),
            typeof(Item),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen),
            typeof(bool),
            typeof(CSOPartEditor),
            new PropertyMetadata(false)
        );

        private static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            nameof(HorizontalOffset),
            typeof(double),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            nameof(VerticalOffset),
            typeof(double),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty FlyoutWidthProperty = DependencyProperty.Register(
            nameof(FlyoutWidth),
            typeof(int),
            typeof(CSOPartEditor),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartEditor;
                    self.LoadWidth = true;
                    self.ScrollViewerWidth = self.FlyoutWidth;
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
            typeof(CSOPartEditor),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartEditor;
                    self.LoadHeight = true;
                    self.ScrollViewerHeight = self.FlyoutHeight - 10;
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
            typeof(CSOPartEditor),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartEditor;
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
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty PartItemsProperty = DependencyProperty.Register(
            nameof(PartItems),
            typeof(ObservableCollection<Item>),
            typeof(CSOPartEditor),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartEditor;
                    self.LoadItem = true;
                    if (self.LoadWidth && self.LoadHeight && self.LoadColor && self.LoadItem)
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        private static readonly DependencyProperty ScrollViewerWidthProperty = DependencyProperty.Register(
            nameof(ScrollViewerWidth),
            typeof(double),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty ScrollViewerHeightProperty = DependencyProperty.Register(
            nameof(ScrollViewerHeightProperty),
            typeof(double),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private void OnPartSelectionChanged(object sender, RoutedEventArgs e)
        {
            var view = sender as CSOPartView;
            if (view != null)
            {
                CurrentPart = view.PartItem;
                IsOpen = false;
            }
        }
    }
}
