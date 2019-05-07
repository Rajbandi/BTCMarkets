using BtcMarkets.Wallet.Droid.Renderers;
using BtcMarkets.Wallet.Helpers;
using Refractored.XamForms.PullToRefresh;
using Refractored.XamForms.PullToRefresh.Droid;
using System;
using System.ComponentModel;
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

        
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);
            }
            catch(Exception ex)
            {

            }
            
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