using CSODataCore;
using CSOLauncher.ViewModels;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Diagnostics;
using Windows.Foundation;

namespace CSOLauncher.View
{
    public sealed partial class CSOReinforceView : UserControl
    {
        public CSOReinforceViewModel ViewModel;

        private double CSOReinforceHorizontalOffset
        {
            get => (double)GetValue(CSOReinforceHorizontalOffsetProperty);
            set => SetValue(CSOReinforceHorizontalOffsetProperty, value);
        }
        private double CSOReinforceVerticalOffset
        {
            get => (double)GetValue(CSOReinforceVerticalOffsetProperty);
            set => SetValue(CSOReinforceVerticalOffsetProperty, value);
        }

        private static readonly DependencyProperty CSOReinforceHorizontalOffsetProperty = DependencyProperty.Register(
            nameof(CSOReinforceHorizontalOffset),
            typeof(double),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        private static readonly DependencyProperty CSOReinforceVerticalOffsetProperty = DependencyProperty.Register(
            nameof(CSOReinforceVerticalOffset),
            typeof(double),
            typeof(CSOPartView),
            new PropertyMetadata(null)
        );

        public CSOReinforceView()
        {
            this.InitializeComponent();
            ReinforceData data = new()
            {
                Damage = 0,
                Accuracy = 0,
                Weight = 0,
                Rebound = 0,
                Repeatedly = 0,
                Ammo = 0,
                OverDmg = 0,

                Reinforce = new()
                {
                    TotalMaxLv = 8,
                    Damage = 5,
                    Accuracy = 5,
                    Weight = 5,
                    Rebound = 5,
                    Repeatedly = 2,
                    Ammo = 1,
                    OverDmg = 20,
                }
            };
            ViewModel = new CSOReinforceViewModel(data)
            {
                IsEditorMode = false,
            };
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ViewModel.CSOReinforceFlyoutIsOpen = true;
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(this);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                ViewModel.CSOReinforceFlyoutIsOpen = true;
                ViewModel.IsEditorMode = !ViewModel.IsEditorMode;
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (ViewModel.IsEditorMode == false)
            {
                ViewModel.CSOReinforceFlyoutIsOpen = false;
            }
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
            if (ViewModel.CSOReinforceFlyoutIsOpen && ViewModel.IsEditorMode == false)
            {
                var pointerPoint = e.GetCurrentPoint(this);
                Point position = pointerPoint.Position;
                //var rect = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
                //在右侧有足够的空间
                //if( rect.Width - position.X + rect.X > CSOPartFlyoutWidth)
                //{
                CSOReinforceHorizontalOffset = position.X;
                CSOReinforceVerticalOffset = position.Y + 1;
                //}
                //else
                //{
                //CSOFlyout.CSOPartHorizontalOffset = position.X + CSOPartFlyoutWidth;
                //CSOFlyout.CSOPartVerticalOffset = position.Y + 1;
                //}
            }
        }

        public void LeftPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var image = (Image)sender;
            if (image != null)
            {
                var data = (ReinforceDataView)image.Tag;
                data.CurrentButtonLeft = ReinforceDataView.ButtonLeft[2];
            }
        }

        public void LeftPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(sender as UIElement);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                var image = (Image)sender;
                if (image != null)
                {
                    var data = (ReinforceDataView)image.Tag;
                    data.CurrentButtonLeft = ReinforceDataView.ButtonLeft[0];
                }
            }
        }

        public void LeftPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(sender as UIElement);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                var image = (Image)sender;
                if (image != null)
                {
                    var data = (ReinforceDataView)image.Tag;
                    data.CurrentButtonLeft = ReinforceDataView.ButtonLeft[2];
                    data.CurrentValue--;
                }
            }
        }

        public void LeftPointerExited(object sender, PointerRoutedEventArgs e)
        {
            var image = (Image)sender;
            if (image != null)
            {
                var data = (ReinforceDataView)image.Tag;
                data.CurrentButtonLeft = ReinforceDataView.ButtonLeft[1];
            }
        }

        public void LeftPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            LeftPointerExited(sender, e);
        }

        public void LeftPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            LeftPointerExited(sender, e);
        }

        public void RightPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var image = (Image)sender;
            if (image != null)
            {
                var data = (ReinforceDataView)image.Tag;
                data.CurrentButtonRight = ReinforceDataView.ButtonRight[2];
            }
        }

        public void RightPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(sender as UIElement);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                var image = (Image)sender;
                if (image != null)
                {
                    var data = (ReinforceDataView)image.Tag;
                    data.CurrentButtonRight = ReinforceDataView.ButtonRight[0];
                }
            }
        }

        public void RightPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var pointerPoint = e.GetCurrentPoint(sender as UIElement);
            if (pointerPoint.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                var image = (Image)sender;
                if (image != null)
                {
                    var data = (ReinforceDataView)image.Tag;
                    data.CurrentButtonRight = ReinforceDataView.ButtonRight[2];
                    data.CurrentValue++;
                }
            }
        }

        public void RightPointerExited(object sender, PointerRoutedEventArgs e)
        {
            var image = (Image)sender;
            if (image != null)
            {
                var data = (ReinforceDataView)image.Tag;
                data.CurrentButtonRight = ReinforceDataView.ButtonRight[1];
            }
        }

        public void RightPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            RightPointerExited(sender, e);
        }

        public void RightPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            RightPointerExited(sender, e);
        }
    }
}
