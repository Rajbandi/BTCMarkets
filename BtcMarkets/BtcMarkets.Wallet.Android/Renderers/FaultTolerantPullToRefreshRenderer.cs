using BtcMarkets.Wallet.Droid.Renderers;
using Refractored.XamForms.PullToRefresh;
using Refractored.XamForms.PullToRefresh.Droid;
using System;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(PullToRefreshLayout), typeof(FaultTolerantPullToRefreshRenderer))]
namespace BtcMarkets.Wallet.Droid.Renderers
{
    public class FaultTolerantPullToRefreshRenderer : PullToRefreshLayoutRenderer
    {
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