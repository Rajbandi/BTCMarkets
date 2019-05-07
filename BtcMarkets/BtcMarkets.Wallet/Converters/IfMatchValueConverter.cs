using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace BtcMarkets.Wallet.Converters
{
    public class IfMatchValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = (string)parameter;
            var v = (string)value;

            if (p == null)
                p = "";

            p = p.ToLower();

            if (v == null)
                v = "";
            v = v.ToLower();

            var values = v.Split(",".ToCharArray());

            return (v == p || values.Contains(p));
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }


        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
