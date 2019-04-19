using BtcMarkets.Wallet.Controls;
using BtcMarkets.Wallet.iOS.Renderers;
using iOSCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MarketChart), typeof(MarketChartRenderer))]

namespace BtcMarkets.Wallet.iOS.Renderers
{
    public class MarketChartRenderer : ViewRenderer<MarketChart, CandleStickChartView>
    {
        private CandleStickChartView candleChart;
        public MarketChartRenderer() : base()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<MarketChart> e)
        {
            base.OnElementChanged(e);
            candleChart = new CandleStickChartView();
            var arr = new List<iOSCharts.CandleChartDataEntry>();
            arr.Add(new CandleChartDataEntry(0, 225.0f, 219.84f, 224.94f, 221.07f));
            arr.Add(new CandleChartDataEntry(1, 228.35f, 222.57f, 223.52f, 226.41f));
            arr.Add(new CandleChartDataEntry(2, 226.84f, 222.52f, 225.75f, 223.84f));
            arr.Add(new CandleChartDataEntry(3, 222.95f, 217.27f, 222.15f, 217.88f));
            arr.Add(new CandleChartDataEntry(4, 225.0f, 219.84f, 224.94f, 221.07f));
            arr.Add(new CandleChartDataEntry(5, 228.35f, 222.57f, 223.52f, 226.41f));
            arr.Add(new CandleChartDataEntry(6, 226.84f, 222.52f, 225.75f, 223.84f));
            arr.Add(new CandleChartDataEntry(7, 222.95f, 217.27f, 222.15f, 217.88f));

            var set1 = new iOSCharts.CandleChartDataSet(arr.ToArray(), "DataSet1");
            set1.SetColor(Color.Yellow.ToUIColor(), 100);
            set1.ShadowColor = Color.Gray.ToUIColor();
            set1.ShadowWidth = 0.8f;
            set1.DecreasingColor = Color.Red.ToUIColor();
            set1.DecreasingFilled = true;
            set1.IncreasingColor = Color.Green.ToUIColor();
            set1.IncreasingFilled = true;
            set1.NeutralColor = Color.LightGray.ToUIColor();
            set1.DrawValuesEnabled = false;

            var candleData = new CandleChartData(new[] { set1 });


            candleChart.HighlightPerDragEnabled = true;
            candleChart.HighlightPerTapEnabled = true;
            candleChart.DrawBordersEnabled = true;
            candleChart.BorderColor = Color.LightGray.ToUIColor();

            ChartYAxis yAxis = candleChart.LeftAxis;
            ChartYAxis rightAxis = candleChart.RightAxis;
            yAxis.DrawGridLinesEnabled =false;
            yAxis.DrawLabelsEnabled = true;

            rightAxis.DrawGridLinesEnabled = false;


            ChartXAxis xAxis = candleChart.XAxis;
            xAxis.DrawLabelsEnabled = false;
            xAxis.LabelTextColor = Color.White.ToUIColor();
            xAxis.Granularity = 1f;
            xAxis.GranularityEnabled = true;
            xAxis.AvoidFirstLastClippingEnabled = true;

            candleChart.Legend.Enabled = false;
            candleChart.Data = candleData;


            SetNativeControl(candleChart);
        }
    }
}