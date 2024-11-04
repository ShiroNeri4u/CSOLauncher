using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace CSOLauncher
{
    public sealed partial class PartButton : UserControl
    {
        private CSOFlyoutBase CSOFlyout;


        public PartButton()
        {
            Item item = ItemManager.Search(1)[0];
            this.InitializeComponent();
            BackGround.Source = Launcher.Assets["empty_slot"];
            CSOFlyout = new CSOFlyoutBase()
            {
                Item = item,
                FlyoutHeight = 300,
                FlyoutWidth = 145,
            };
            CSOFlyout.OnLoad();
            CSOFlyout.Flyout.Content = new TextBlock
            {
               Width = 145,
               Height = 300,
               Text = "≤‚ ‘",
            };
            Flyout.SetAttachedFlyout(Button, CSOFlyout.Flyout);
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CSOFlyout.Flyout.ShowAt(Button);
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            CSOFlyout.Flyout.Hide();
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            CSOFlyout.Flyout.ShowAt(Button);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            CSOFlyout.Flyout.Hide();
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
        }
    }
}
