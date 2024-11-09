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
        private WriteableBitmap CSOPartEditorBackground
        {
            get => (WriteableBitmap)GetValue(CSOPartEditorBackgroundProperty);
            set => SetValue(CSOPartEditorBackgroundProperty, value);
        }

        private int CSOPartEditorScrollViewerWidth
        {
            get => (int)GetValue(CSOPartEditorScrollViewerWidthProperty);
            set => SetValue(CSOPartEditorScrollViewerWidthProperty, value);
        }

        private int CSOPartEditorScrollViewerHeight
        {
            get => (int)GetValue(CSOPartEditorScrollViewerHeightProperty);
            set => SetValue(CSOPartEditorScrollViewerHeightProperty, value);
        }

        public double CSOPartEditorHorizontalOffset
        {
            get => (double)GetValue(CSOPartEditorHorizontalOffsetProperty);
            set => SetValue(CSOPartEditorHorizontalOffsetProperty, value);
        }

        public double CSOPartEditorVerticalOffset
        {
            get => (double)GetValue(CSOPartEditorVerticalOffsetProperty);
            set => SetValue(CSOPartEditorVerticalOffsetProperty, value);
        }

        public int CSOPartEditorWidth
        {
            get => (int)GetValue(CSOPartEditorWidthProperty);
            set => SetValue(CSOPartEditorWidthProperty, value);
        }

        public int CSOPartEditorHeight
        {
            get => (int)GetValue(CSOPartEditorHeightProperty);
            set => SetValue(CSOPartEditorHeightProperty, value);
        }
        
        public bool CSOPartEditorIsOpen
        {
            get => (bool)GetValue(CSOPartEditorIsOpenProperty);
            set => SetValue(CSOPartEditorIsOpenProperty, value);
        }

        public CSOFlyoutBase.Color CSOPartEditorBorderColor
        {
            get => (CSOFlyoutBase.Color)GetValue(CSOPartEditorBorderColorProperty);
            set => SetValue(CSOPartEditorBorderColorProperty, value);
        }

        public Item CSOPartItem
        {
            get => (Item)GetValue(CSOPartItemProperty);
            set => SetValue(CSOPartItemProperty, value);
        }

        public List<Item> CSOPartItems
        {
            get => (List<Item>)GetValue(CSOPartItemsProperty);
            set => SetValue(CSOPartItemsProperty, value);
        }

        public CSOPartEditor()
        {
            this.InitializeComponent();
        }

        private void OnLoad()
        {
            Set(CSOFlyoutBase.GetAssets(CSOPartEditorWidth, CSOPartEditorHeight, CSOPartEditorBorderColor));
        }

        private void Set(string taskname)
        {
            CSOPartEditorBackground = Launcher.Assets[taskname];
        }

        private static readonly DependencyProperty CSOPartEditorBackgroundProperty = DependencyProperty.Register(
            nameof(CSOPartEditorBackground),
            typeof(WriteableBitmap),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartEditorScrollViewerWidthProperty = DependencyProperty.Register(
            nameof(CSOPartEditorScrollViewerWidth),
            typeof(int),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartEditorScrollViewerHeightProperty = DependencyProperty.Register(
            nameof(CSOPartEditorScrollViewerHeight),
            typeof(int),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartEditorHorizontalOffsetProperty = DependencyProperty.Register(
            nameof(CSOPartEditorHorizontalOffset),
            typeof(double),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartEditorVerticalOffsetProperty = DependencyProperty.Register(
            nameof(CSOPartEditorVerticalOffset),
            typeof(double),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartEditorWidthProperty = DependencyProperty.Register(
            nameof(CSOPartEditorWidth),
            typeof(double),
            typeof(CSOPartEditor),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartEditor;
                    self.LoadWidth = true;
                    self.CSOPartEditorScrollViewerWidth = self.CSOPartEditorWidth;
                    if (self.LoadWidth && self.LoadHeight && self.LoadColor && self.LoadItem)
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        private static readonly DependencyProperty CSOPartEditorHeightProperty = DependencyProperty.Register(
            nameof(CSOPartEditorHeight),
            typeof(double),
            typeof(CSOPartEditor),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartEditor;
                    self.LoadHeight = true;
                    self.CSOPartEditorScrollViewerHeight = self.CSOPartEditorHeight - 10;
                    if (self.LoadWidth && self.LoadHeight && self.LoadColor && self.LoadItem)
                    {
                        self.OnLoad();
                    }
                }
            )
        );

        public static readonly DependencyProperty CSOPartEditorIsOpenProperty = DependencyProperty.Register(
            nameof(CSOPartEditorIsOpen),
            typeof(bool),
            typeof(CSOPartEditor),
            new PropertyMetadata(false)
        );

        private static readonly DependencyProperty CSOPartEditorBorderColorProperty = DependencyProperty.Register(
            nameof(CSOPartEditorBorderColor),
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

        public static readonly DependencyProperty CSOPartItemProperty = DependencyProperty.Register(
            nameof(CSOPartItem),
            typeof(Item),
            typeof(CSOPartEditor),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartItemsProperty = DependencyProperty.Register(
            nameof(CSOPartItems),
            typeof(List<Item>),
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

        private void OnPartSelectionChanged(object sender, RoutedEventArgs e)
        {
            var view = sender as CSOPartView;
            if (view != null)
            {
                CSOPartItem = view.CSOPartItem;
                CSOPartEditorIsOpen = false;
            }
        }
    }
}
