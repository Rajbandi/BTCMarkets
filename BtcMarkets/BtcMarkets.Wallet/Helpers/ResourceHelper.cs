using BtcMarkets.Wallet.Models;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Helpers
{
    public static class ResourceHelper
    {
        public static Color PrimaryColor
        {
            get
            {
                var color = (Color) Application.Current.Resources["PrimaryColor"];
                return color;
            }
        }
        public static Color SecondaryColor
        {
            get
            {
                var color = (Color)Application.Current.Resources["SecondaryColor"];
                return color;
            }
        }

        public static CoinConfig GetCoinConfig()
        {
            CoinConfig config;
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(CoinConfig)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("BtcMarkets.Wallet.Config.settings.json");
            var text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            config = CoinConfig.FromJson(text);

            return config;
        }

        public static string GetChartHtml()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(CoinConfig)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("BtcMarkets.Wallet.Resources.chart.html");
            var text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }
    }
}
