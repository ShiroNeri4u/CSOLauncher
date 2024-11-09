using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace CSOLauncher
{
    public sealed partial class ItemView : UserControl
    {
        public ItemData Data
        {
            get => (ItemData)GetValue(CSOItemDataProperty);
            set => SetValue(CSOItemDataProperty, value);
        }
        private BitmapImage CurrentImage
        {
            get => (BitmapImage)GetValue(CSOItemDataProperty);
            set => SetValue(CSOItemDataProperty, value);
        }
        public ItemView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty CSOItemDataProperty = DependencyProperty.Register(
            nameof(ItemData),
            typeof(ItemData),
            typeof(ItemView),
            new PropertyMetadata(null)
        );
    }
}
