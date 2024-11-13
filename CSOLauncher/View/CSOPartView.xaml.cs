using CSODataCore;
using CSOLauncher.ViewModels;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;
using static CSOLauncher.Launcher;

namespace CSOLauncher.View
{
    public sealed partial class CSOPartView : UserControl
    {
        public CSOPartViewModel ViewModel
        {
            get => (CSOPartViewModel)GetValue(ViewModelProperty); 
            set => SetValue(ViewModelProperty, value);
        }

        public ViewModelPropertyChanged Changed
        {
            get => (ViewModelPropertyChanged)GetValue(ChangedProperty);
            set => SetValue(ChangedProperty, value);
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

        public CSOPartView()
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
            typeof(CSOPartViewModel),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty ChangedProperty = DependencyProperty.Register(
            nameof(Changed),
            typeof(ViewModelPropertyChanged),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!ViewModel.CSOPartEditorIsOpen)
            {
                ViewModel.CSOPartFlyoutIsOpen = true;
            }
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            var pointerPoint = e.GetCurrentPoint(this);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                ViewModel.CSOPartFlyoutIsOpen = false;
            }
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(this);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                ViewModel.CSOPartFlyoutIsOpen = false;
                ViewModel.CSOPartEditorIsOpen = true;
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            ViewModel.CSOPartFlyoutIsOpen = false;
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
            if (ViewModel.CSOPartFlyoutIsOpen)
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

        private void SelectPartItem(object sender, TappedRoutedEventArgs e)
        {
            
            var grid = (Grid)sender;
            if (grid != null)
            {
                var data = (Item)grid.Tag;
                if(ViewModel.Slot == CSOPartViewModel.CSOPartSlot.Part1)
                {
                    ViewModel.CSOItemData.Part1 = data;
                }
                else
                {
                    ViewModel.CSOItemData.Part2 = data;
                }
                ViewModel.Changed();
            }
            ViewModel.CSOPartEditorIsOpen = false;
        }
    }
}
