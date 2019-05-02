using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Converters
{


    public class ToggleImageConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FontImageSource source = null;
            try
            {
                string icon = "";

                if (value != null && value is ToggleImage)
                {
                    var toggle = (ToggleImage)value;
                    var fontFamily = toggle.FontFamily;
                    if(string.IsNullOrWhiteSpace(fontFamily))
                    {
                        fontFamily = "MaterialDesign";
                    }
                    var resources = Application.Current.Resources;
                    icon = toggle.Value ? toggle.OnImage : toggle.OffImage;

                    source = new FontImageSource();
                    var font = (OnPlatform<string>)resources[fontFamily];
                    source.FontFamily = font;
                    source.Glyph = icon;

                    if (!string.IsNullOrWhiteSpace(toggle.Color))
                    {
                        source.Color = (Color)resources[toggle.Color];
                    }
                }

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
