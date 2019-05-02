using System.IO;
using BtcMarkets.Wallet.Droid.Services;
using BtcMarkets.Wallet.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformService))]
namespace BtcMarkets.Wallet.Droid.Services
{
    public class PlatformService : IPlatformService
    {
        //public string GetChartHtml()
        //{
        //    string html = "";
        //    var assetManager = Android.App.Application.Context.Assets;
        //    using (var streamReader = new StreamReader(assetManager.Open("chart.html")))
        //    {
        //        html = streamReader.ReadToEnd();
        //    }
        //    return html;
        //}

        public string GetHtmlBasePath()
        {
            return "file:///android_asset/";
        }
    }
}