using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace BtcMarkets.Wallet.Droid.Controls
{
    public class MarketCandleStickChart : CandleStickChart
    {
        public MarketCandleStickChart(Context context) : base(context)
        {

            LoadData();

        }


        public void LoadData()
        {
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
            set1.SetColor(Color.Yellow, 100);
            set1.ShadowColor = Color.Gray;
            set1.ShadowWidth = 0.8f;
            set1.DecreasingColor = Color.Red;
            set1.DecreasingPaintStyle = Paint.Style.Fill;
            set1.IncreasingColor = Color.Green;
            set1.IncreasingPaintStyle = Paint.Style.Fill;
            set1.NeutralColor = Color.LightGray;
            set1.SetDrawValues(false);

            var candleData = new CandleData(set1);

          
            this.HighlightPerDragEnabled = true;
            this.HighlightPerTapEnabled = true;
            this.SetDrawBorders(true);
            this.SetBorderColor(Color.LightGray);

            YAxis yAxis = this.AxisLeft;
            YAxis rightAxis = this.AxisRight;
            yAxis.SetDrawGridLines(false);
            yAxis.SetDrawLabels(true);

            rightAxis.SetDrawGridLines(false);

            this.RequestDisallowInterceptTouchEvent(true);

            XAxis xAxis = XAxis;
            xAxis.SetDrawLabels(false);
            xAxis.TextColor = Color.White;
            xAxis.Granularity = 1f;
            xAxis.GranularityEnabled = true;
            xAxis.SetAvoidFirstLastClipping(true);

            Legend.Enabled = false;
            this.Data = candleData;
            this.SetMinimumHeight(400);
            this.SetMinimumWidth(400);
           // this.Invalidate();
        }
    }
}