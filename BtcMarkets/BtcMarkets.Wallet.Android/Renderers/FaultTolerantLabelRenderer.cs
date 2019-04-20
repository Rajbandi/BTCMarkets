using BtcMarkets.Wallet.Droid.Renderers;
using Refractored.XamForms.PullToRefresh;
using Refractored.XamForms.PullToRefresh.Droid;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(FaultTolerantLabelRenderer))]
namespace BtcMarkets.Wallet.Droid.Renderers
{
    public class FaultTolerantLabelRenderer : LabelRenderer
    {
        public FaultTolerantLabelRenderer() : base(Android.App.Application.Context)
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