using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CSOLauncher
{
    public sealed partial class ItemView : UserControl
    {
        public ItemView()
        {
            this.InitializeComponent();
        }

        public ItemData ItemData
        {
            get => (ItemData)GetValue(CSOItemDataProperty);
            set => SetValue(CSOItemDataProperty, value);
        }

        public static readonly DependencyProperty CSOItemDataProperty = DependencyProperty.Register(
            nameof(ItemData),
            typeof(ItemData),
            typeof(ItemView),
            new PropertyMetadata(null)
        );
    }
}
