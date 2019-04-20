using BtcMarkets.Wallet.ViewModels;
using BtcMarkets.Wallet.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Converters
{
    public class TradeSelectedStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            var styleName = "SmallDefaultText";

           
            if( value != null)
            {
                var arg = parameter as TradesPage;
                if (arg != null)
                {
                    var viewModel = arg.BindingContext as TradesViewModel;

                    if (viewModel != null)
                    {
                      
                    }
                }
            }

            var style = (Style)Application.Current.Resources[styleName];
            return style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }


    }
}
