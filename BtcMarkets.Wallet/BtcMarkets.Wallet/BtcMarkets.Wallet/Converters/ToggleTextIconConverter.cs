using BtcMarkets.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Converters
{


    public class ToggleTextIconConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = null;
            try
            {
                

                if (value != null && value is ToggleImage)
                {
                    var toggle = (ToggleImage)value;

                    source = toggle.Value ? toggle.OnImage : toggle.OffImage;
                }

            }
            catch (Exception ex)
            {
                Helpers.AppHelper.TrackError(ex);
            }
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


    }

}
