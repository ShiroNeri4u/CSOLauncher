using CSODataCore;
using CSOLauncher.ViewModels;
using Microsoft.UI.Content;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;
using static CSOLauncher.Launcher;

namespace CSOLauncher.View
{
    public sealed partial class CSOPaintView : UserControl
    {
        public CSOPaintViewModel ViewModel
        {
            get => (CSOPaintViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
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

        public CSOPaintView()
        {
            this.InitializeComponent();
        }

        private static readonly DependencyProperty CSOPartHorizontalOffsetProperty = DependencyProperty.Register(
            nameof(CSOPartHorizontalOffset),
            typeof(double),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOPartVerticalOffsetProperty = DependencyProperty.Register(
            nameof(CSOPartVerticalOffset),
            typeof(double),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(CSOPaintViewModel),
            typeof(CSOPaintView),
            new PropertyMetadata(null)
        );

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            if (!ViewModel.CSOPaintEditorIsOpen)
            {
                ViewModel.CSOPaintFlyoutIsOpen = true;
            }
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(this);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                ViewModel.CSOPaintFlyoutIsOpen = false;
            }
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(this);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                ViewModel.CSOPaintFlyoutIsOpen = false;
                ViewModel.CSOPaintEditorIsOpen = true;
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            ViewModel.CSOPaintFlyoutIsOpen = false;
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
            if (ViewModel.CSOPaintFlyoutIsOpen)
            {
                var pointerPoint = e.GetCurrentPoint(this);
                Point position = pointerPoint.Position;
                //var rect = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
                //���Ҳ����㹻�Ŀռ�
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

        private void SelectPartItem(object sender, TappedRoutedEventArgs e)
        {
            var grid = (Grid)sender;
            if (grid != null)
            {
                var data = (Item)grid.Tag;
                ViewModel.CSOItemData.Paint = data;
                ViewModel.Changed?.Invoke();
            }
            ViewModel.CSOPaintEditorIsOpen = false;
        }
    }
}
