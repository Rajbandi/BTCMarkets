using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BtcMarkets.Wallet.Droid.Renderers;
using Xamarin.Forms;
using BtcMarkets.Wallet.Controls;
using MikePhil.Charting.Charts;

[assembly: ExportRenderer(typeof(BtcMarkets.Wallet.Controls.Chart), typeof(ChartRenderer))]
namespace BtcMarkets.Wallet.Droid.Renderers
{
    public class ChartRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Wallet.Controls.Chart, CandleStickChart>
    {
        public ChartRenderer(Context context) : base(context)
        {
            CandleStickChart chart = new CandleStickChart(context);
            MikePhil.Charting.Charts.Chart ch = (MikePhil.Charting.Charts.Chart)chart;

         
        }
    }
}