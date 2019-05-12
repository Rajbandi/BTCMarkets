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
                Color color = Color.Default;
                var resources = Application.Current.Resources;
                if (value != null && value is string)
                {
                    icon = (string)value;
                }
                else
                if (parameter != null && parameter is string)
                {
                    var p = (string)parameter;
                    var parts = p.Split("|".ToCharArray());
                    if (parts.Length > 0)
                    {
                        icon = parts[0];
                        if(parts.Length>1)
                        {
                            var colorCode = parts[1];
                            if (colorCode.StartsWith("#"))
                            {
                                color = Color.FromHex(colorCode);
                            }
                            else
                            {
                                color = (Color)resources[parts[1]];
                            }
                        }
                    }
                }

                source = new FontImageSource();

                var font = (OnPlatform<string>)Application.Current.Resources["MaterialDesign"];
                source.FontFamily = font;
                source.Glyph = icon;
                if(color != null && color != Color.Default )
                {
                    var hex = color.ToHexWeb();
                    source.Color = color;
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
