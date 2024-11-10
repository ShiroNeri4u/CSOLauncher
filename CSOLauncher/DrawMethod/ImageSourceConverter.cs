using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace CSOLauncher.DrawMethod
{
    public partial class ImageSourceConverter : IValueConverter
    {
        object? IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BitmapImage)
            {
                BitmapImage self = (BitmapImage)value;
                if(self != null)
                {
                    return self;
                }
                else return value;
            }
            else if (value is WriteableBitmap self)
            {
                if (self != null)
                {
                    return self;
                }
                else
                {
                    return value;
                }
            }
            else return null;
        }

        object? IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}
