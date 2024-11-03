using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace CSOLauncher
{
    public sealed partial class PartButton : UserControl
    {
        private ToolTip PartInformation;
        public PartButton()
        {
            this.InitializeComponent();
            BackGround.Source = Launcher.Assets["empty_slot"];
            PartInformation = new ToolTip()
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = "This is a custom flyout!" },
                    }
                },
            };
            ToolTipService.SetToolTip(this, PartInformation);
        }

        public event RoutedEventHandler Click;

        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register(
            nameof(Click),
            typeof(RoutedEventHandler),
            typeof(CSOButton),
            new PropertyMetadata(null)
        );

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
        {
        }

        private void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
        }
    }
}
