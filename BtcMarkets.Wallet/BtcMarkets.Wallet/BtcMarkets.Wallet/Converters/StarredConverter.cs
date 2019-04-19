using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Converters
{
    public class StarredConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool )
            {
                if((bool)value == true)
                {
                    return "star";
                }
            }

            return "star_border";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                return (string)value == "star";
            }
            return false;
        }
    }
}
