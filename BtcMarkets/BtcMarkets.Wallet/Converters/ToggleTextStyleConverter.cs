using BtcMarkets.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Converters
{


    public class ToggleTextStyleConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Style source = null;
            var resources = Application.Current.Resources;
            try
            {
                if (value != null && value is bool)
                {
                    var v = (bool)value;
                    var p = (string)parameter;
                    if (!string.IsNullOrWhiteSpace(p))
                    {
                        var styles = p.Split("|".ToCharArray());
                        if (styles.Length > 1)
                        {
                            var s = v ? styles[0] : styles[1];
                            source = (Style)resources[s];
                        }

                    }
                }

            }
            catch (Exception ex)
            {
              
            }
            if(source == null && value != null)
            {
                source = (Style)resources["DefaultText"];
            }
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


    }

}
