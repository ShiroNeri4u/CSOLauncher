using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOLauncher
{
    public partial class ImageSourceConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is BitmapImage)
            {
                var self = value as BitmapImage;
                return self;
            }
            else if (value is WriteableBitmap)
            {
                var self = value as WriteableBitmap;
                return self;
            }
            else return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}
