using BtcMarkets.Wallet.Droid.Renderers;
using Refractored.XamForms.PullToRefresh;
using Refractored.XamForms.PullToRefresh.Droid;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Image), typeof(FaultTolerantImageRenderer))]
namespace BtcMarkets.Wallet.Droid.Renderers
{
    public class FaultTolerantImageRenderer : ImageRenderer
    {
        public FaultTolerantImageRenderer() : base(Android.App.Application.Context)
        {

        }
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
                Tracker?.Dispose();

            }
            catch (Exception ex)
            {

            }
        }
    }
}