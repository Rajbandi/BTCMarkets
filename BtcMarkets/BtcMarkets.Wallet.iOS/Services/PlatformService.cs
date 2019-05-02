using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BtcMarkets.Wallet.iOS.Services;
using BtcMarkets.Wallet.Services;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformService))]
namespace BtcMarkets.Wallet.iOS.Services
{
    public class PlatformService : IPlatformService
    {
        

        public string GetHtmlBasePath()
        {
           return NSBundle.MainBundle.BundlePath;
        }
    }
}