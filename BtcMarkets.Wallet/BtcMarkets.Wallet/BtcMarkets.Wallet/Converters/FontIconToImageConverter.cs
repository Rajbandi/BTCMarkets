using BtcMarkets.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Converters
{
    public class FontIconToImageConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FontImageSource source = null;
            try
            {
                string icon = "";
                if (value != null && value is string)
                {
                    icon = (string)value;
                }
                else
                if (parameter != null && parameter is string)
                {
                    icon = (string)parameter;
                }

                source = new FontImageSource();

                var font = (OnPlatform<string>)Application.Current.Resources["MaterialDesign"];
                source.FontFamily = font;
                source.Glyph = icon;

            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


    }

}
