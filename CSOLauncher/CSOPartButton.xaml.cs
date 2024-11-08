using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
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

        public Item PartItem
        {
            get => (Item)GetValue(CSOItemProperty);
            set => SetValue(CSOItemProperty, value);
        }

        public CSOPartButton()
        {
            this.InitializeComponent();
            CurrentBackground = Launcher.Assets["empty_slot"];
        }

        public void LoadFlyout()
        {
        }

        private static void CalaHeight()
        {

        }

        public static readonly DependencyProperty CurrentBackgroundProperty = DependencyProperty.Register(
            nameof(CurrentBackground),
            typeof(WriteableBitmap),
            typeof(CSOFlyoutBase),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty CSOItemProperty = DependencyProperty.Register(
            nameof(PartItem),
            typeof(Item),
            typeof(CSOFlyoutBase),
            new PropertyMetadata(null,
                static (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                {
                    
                }
            )
        );

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CSOFlyout.IsOpen = true;
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            CSOFlyout.IsOpen = false;
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CSOFlyout.IsOpen = true;
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            CSOFlyout.IsOpen = false;
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
            if (CSOFlyout.IsOpen)
            {
                var pointerPoint = e.GetCurrentPoint(this);
                Point position = pointerPoint.Position;
                //var rect = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
                //在右侧有足够的空间
                //if( rect.Width - position.X + rect.X > FlyoutWidth)
                //{
                    CSOFlyout.HorizontalOffset = position.X;
                    CSOFlyout.VerticalOffset = position.Y + 1;
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
