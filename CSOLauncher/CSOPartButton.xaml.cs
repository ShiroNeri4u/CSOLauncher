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
        private WriteableBitmap CurrentBackground
        {
            get => (WriteableBitmap)GetValue(CurrentBackgroundProperty);
            set => SetValue(CurrentBackgroundProperty, value);
        }
        private Item PartItem
        {
            get => (Item)GetValue(CSOPartItemProperty);
            set => SetValue(CSOPartItemProperty, value);
        }
        private ObservableCollection<Item> PartItems
        {
            get => (ObservableCollection<Item>)GetValue(PartItemsProperty);
            set => SetValue(PartItemsProperty, value);
        }

        private bool FlyoutIsOpen
        {
            get => (bool)GetValue(FlyoutIsOpenProperty);
            set => SetValue(FlyoutIsOpenProperty, value);
        }

        private bool EditorIsOpen
        {
            get => (bool)GetValue(EditorIsOpenProperty);
            set => SetValue(EditorIsOpenProperty, value);
        }

        private double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        private double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }

        public CSOPartButton()
        {
            this.InitializeComponent();
            PartItem = ItemManager.EmptyItem;
        }

        public static readonly DependencyProperty CurrentBackgroundProperty = DependencyProperty.Register(
            nameof(CurrentBackground),
            typeof(WriteableBitmap),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty CSOPartItemProperty = DependencyProperty.Register(
            nameof(PartItem),
            typeof(Item),
            typeof(CSOPartButton),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    var self = obj as CSOPartButton;
                    if (!self.PartItem.IsEmpty)
                    {
                        if(Launcher.Assets.TryGetValue(self.PartItem.ResourceName.ToLower() + "_s", out WriteableBitmap bitmap))
                        {
                            self.CurrentBackground = bitmap;
                        }
                        else
                        {
                            self.CurrentBackground = Launcher.Assets["empty_slot"];
                        }
                    }
                    else
                    {
                        self.CurrentBackground = Launcher.Assets["empty_slot"];
                    }
                    List<Item> items = [];
                    items.Add(ItemManager.EmptyItem);
                    foreach (Item item in ItemManager.Search(ItemPart.WeaponType))
                    {
                        if(!item.IsEmpty)
                        {
                            items.Add(item);
                        }
                    }
                    self.PartItems = [.. items];
                }
            )
        );

        private static readonly DependencyProperty PartItemsProperty = DependencyProperty.Register(
            nameof(PartItems),
            typeof(ObservableCollection<Item>),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty FlyoutIsOpenProperty = DependencyProperty.Register(
            nameof(FlyoutIsOpen),
            typeof(bool),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty EditorIsOpenProperty = DependencyProperty.Register(
            nameof(EditorIsOpen),
            typeof(bool),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            nameof(HorizontalOffset),
            typeof(double),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            nameof(VerticalOffset),
            typeof(double),
            typeof(CSOPartButton),
            new PropertyMetadata(null)
        );

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!EditorIsOpen)
            {
                FlyoutIsOpen = true;
            }
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FlyoutIsOpen = false;
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FlyoutIsOpen = false;
            EditorIsOpen = true;
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            FlyoutIsOpen = false;
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
            if (FlyoutIsOpen)
            {
                var pointerPoint = e.GetCurrentPoint(this);
                Point position = pointerPoint.Position;
                //var rect = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
                //在右侧有足够的空间
                //if( rect.Width - position.X + rect.X > FlyoutWidth)
                //{
                    HorizontalOffset = position.X;
                    VerticalOffset = position.Y + 1;
                //}
                //else
                //{
                    //CSOFlyout.HorizontalOffset = position.X + FlyoutWidth;
                    //CSOFlyout.VerticalOffset = position.Y + 1;
                //}
            }
        }
    }
}
