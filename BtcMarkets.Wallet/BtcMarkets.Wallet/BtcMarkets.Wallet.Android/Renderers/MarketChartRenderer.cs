using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using BtcMarkets.Wallet.Controls;
using BtcMarkets.Wallet.Droid.Renderers;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(MarketChart), typeof(MarketChartRenderer))]

namespace BtcMarkets.Wallet.Droid.Renderers
{
    public class MarketChartRenderer : ViewRenderer<MarketChart, CandleStickChart>
    {
        private CandleStickChart candleChart;
        public MarketChartRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<MarketChart> e)
        {
            base.OnElementChanged(e);
            candleChart = new CandleStickChart(Context);
            var arr = new List<CandleEntry>();
            arr.Add(new CandleEntry(0, 225.0f, 219.84f, 224.94f, 221.07f));
            arr.Add(new CandleEntry(1, 228.35f, 222.57f, 223.52f, 226.41f));
            arr.Add(new CandleEntry(2, 226.84f, 222.52f, 225.75f, 223.84f));
            arr.Add(new CandleEntry(3, 222.95f, 217.27f, 222.15f, 217.88f));
            arr.Add(new CandleEntry(4, 225.0f, 219.84f, 224.94f, 221.07f));
            arr.Add(new CandleEntry(5, 228.35f, 222.57f, 223.52f, 226.41f));
            arr.Add(new CandleEntry(6, 226.84f, 222.52f, 225.75f, 223.84f));
            arr.Add(new CandleEntry(7, 222.95f, 217.27f, 222.15f, 217.88f));

            var set1 = new CandleDataSet(arr, "DataSet1");
            set1.SetColor(Color.Yellow.ToAndroid(), 100);
            set1.ShadowColor = Color.Gray.ToAndroid();
            set1.ShadowWidth = 0.8f;
            set1.DecreasingColor = Color.Red.ToAndroid();
            set1.DecreasingPaintStyle = Paint.Style.Fill;
            set1.IncreasingColor = Color.Green.ToAndroid();
            set1.IncreasingPaintStyle = Paint.Style.Fill;
            set1.NeutralColor = Color.LightGray.ToAndroid();
            set1.SetDrawValues(false);

            var candleData = new CandleData(set1);


            candleChart.HighlightPerDragEnabled = true;
            candleChart.HighlightPerTapEnabled = true;
            candleChart.SetDrawBorders(true);
            candleChart.SetBorderColor(Color.LightGray.ToAndroid());

            YAxis yAxis = candleChart.AxisLeft;
            YAxis rightAxis = candleChart.AxisRight;
            yAxis.SetDrawGridLines(false);
            yAxis.SetDrawLabels(true);

            rightAxis.SetDrawGridLines(false);

            this.RequestDisallowInterceptTouchEvent(true);

            XAxis xAxis = candleChart.XAxis;
            xAxis.SetDrawLabels(false);
            xAxis.TextColor = Color.White.ToAndroid();
            xAxis.Granularity = 1f;
            xAxis.GranularityEnabled = true;
            xAxis.SetAvoidFirstLastClipping(true);

            candleChart.Legend.Enabled = false;
            candleChart.Data = candleData;
           


            SetNativeControl(candleChart);
        }
    }
}