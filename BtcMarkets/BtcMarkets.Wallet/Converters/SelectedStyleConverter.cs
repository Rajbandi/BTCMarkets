using BtcMarkets.Wallet.ViewModels;
using BtcMarkets.Wallet.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Converters
{
    public class SelectedStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultStyle = (string)parameter;

            var targetStyle = (string)value;
        
            if(string.IsNullOrWhiteSpace(targetStyle))
            {
                targetStyle = defaultStyle;
            }

            Style style = null;
            try
            {
                style = (Style)Application.Current.Resources[targetStyle];
            }
            catch(Exception)
            {

            }
            return style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }


    }
}
