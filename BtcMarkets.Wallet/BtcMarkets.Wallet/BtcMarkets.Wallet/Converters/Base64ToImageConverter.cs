using BtcMarkets.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Converters
{
    public class Base64ToImageConverter : IValueConverter
    {
        ImageSource image;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string)
                {
                    image = null;
                    byte[] bytes = System.Convert.FromBase64String(value.ToString());
                    image = ImageSource.FromStream(() => new MemoryStream(bytes));
                    return image;
                }
            }
            catch(Exception ex)
            {
                AppHelper.TrackError(ex);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
