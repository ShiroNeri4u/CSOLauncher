using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation;

namespace CSOLauncher
{
    public sealed partial class CSOPartButton : UserControl
    {
        private const string PostName = "_s";
        private const string EmptySlot = "empty_slot";
        private WriteableBitmap CSOPartFlyoutBorderBackground
        {
            get => (WriteableBitmap)GetValue(CSOPartFlyoutBorderBackgroundProperty);
            set => SetValue(CSOPartFlyoutBorderBackgroundProperty, value);
        }
        private Item CSOPartItem
        {
            get => (Item)GetValue(CSOPartItemProperty);
            set => SetValue(CSOPartItemProperty, value);
        }
        private List<Item> CSOPartItems
        {
            get => (List<Item>)GetValue(CSOPartItemsProperty);
            set => SetValue(CSOPartItemsProperty, value);
        }

        private bool CSOPartFlyoutIsOpen
        {
            get => (bool)GetValue(CSOPartFlyoutIsOpenProperty);
            set => SetValue(CSOPartFlyoutIsOpenProperty, value);
        }

        private bool CSOPartEditorIsOpen
        {
            get => (bool)GetValue(CSOPartEditorIsOpenProperty);
            set => SetValue(CSOPartEditorIsOpenProperty, value);
        }

        private double CSOPartHorizontalOffset
        {
            get => (double)GetValue(CSOPartHorizontalOffsetProperty);
            set => SetValue(CSOPartHorizontalOffsetProperty, value);
        }

        private double CSOPartVerticalOffset
        {
            get => (double)GetValue(CSOPartVerticalOffsetProperty);
            set => SetValue(CSOPartVerticalOffsetProperty, value);
        }

        public CSOPartButton()
        {
            this.InitializeComponent();
            CSOPartItem = ItemManager.EmptyItem;
        }

        public static readonly DependencyProperty CSOPartFlyoutBorderBackgroundProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutBorderBackground),
            typeof(WriteableBitmap),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty CSOPartItemProperty = DependencyProperty.Register(
            nameof(CSOPartItem),
            typeof(Item),
            typeof(CSOPartButton),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartButton;
                    if (!self.CSOPartItem.IsEmpty)
                    {
                        if(Launcher.Assets.TryGetValue(self.CSOPartItem.ResourceName.ToLower() + PostName, out WriteableBitmap bitmap))
                        {
                            self.CSOPartFlyoutBorderBackground = bitmap;
                        }
                        else
                        {
                            self.CSOPartFlyoutBorderBackground = Launcher.Assets[EmptySlot];
                        }
                    }
                    else
                    {
                        self.CSOPartFlyoutBorderBackground = Launcher.Assets[EmptySlot];
                    }
                    self.CSOPartItems = ItemManager.PartDictionary[ItemPart.WeaponType];
                }
            )
        );

        private static readonly DependencyProperty CSOPartItemsProperty = DependencyProperty.Register(
            nameof(CSOPartItems),
            typeof(List<Item>),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty CSOPartFlyoutIsOpenProperty = DependencyProperty.Register(
            nameof(CSOPartFlyoutIsOpen),
            typeof(bool),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty CSOPartEditorIsOpenProperty = DependencyProperty.Register(
            nameof(CSOPartEditorIsOpen),
            typeof(bool),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartHorizontalOffsetProperty = DependencyProperty.Register(
            nameof(CSOPartHorizontalOffset),
            typeof(double),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartVerticalOffsetProperty = DependencyProperty.Register(
            nameof(CSOPartVerticalOffset),
            typeof(double),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!CSOPartEditorIsOpen)
            {
                CSOPartFlyoutIsOpen = true;
            }
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            CSOPartFlyoutIsOpen = false;
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CSOPartFlyoutIsOpen = false;
            CSOPartEditorIsOpen = true;
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            CSOPartFlyoutIsOpen = false;
        }

        private void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            OnPointerExited(sender, e);
        }

        private void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            OnPointerExited(sender, e);
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (CSOPartFlyoutIsOpen)
            {
                var pointerPoint = e.GetCurrentPoint(this);
                Point position = pointerPoint.Position;
                //var rect = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
                //在右侧有足够的空间
                //if( rect.Width - position.X + rect.X > CSOPartFlyoutWidth)
                //{
                    CSOPartHorizontalOffset = position.X;
                    CSOPartVerticalOffset = position.Y + 1;
                //}
                //else
                //{
                    //CSOFlyout.CSOPartHorizontalOffset = position.X + CSOPartFlyoutWidth;
                    //CSOFlyout.CSOPartVerticalOffset = position.Y + 1;
                //}
            }
        }
    }
}
