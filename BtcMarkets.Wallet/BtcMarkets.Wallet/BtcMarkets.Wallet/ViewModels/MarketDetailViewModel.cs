using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using Newtonsoft.Json;
//using Microcharts;
using SkiaSharp;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;

namespace BtcMarkets.Wallet.ViewModels
{
    [JsonObject]
    public class ChartEntry
    {
        [JsonProperty("x")]
        public string X { get; set; }

        [JsonProperty("y")]
        public double[] Y { get; set; }
    }
    public class MarketDetailViewModel : BaseViewModel
    {

        public Market Market { get; private set; }
        public MarketDetailViewModel(Market market)
        {
            Market = market;
            Title = $"{market.Name}({market.Pair})";
            ChartData = new List<HighLowItem>();

        }


        private string _reportHtml;
        public string ReportHtml
        {
            get
            {
                return _reportHtml;
            }
            set
            {
                _reportHtml = value;
                OnPropertyChanged(nameof(ReportHtml));
            }
        }

        public async Task LoadReport()
        {
            IsBusy = true;
            var data = await AppData.Current.GetMarketHistory(Market);
            ChartData.Clear();
            foreach (var entry in data)
            {
                double t = DateTimeAxis.ToDouble(entry.Date);
                var item = new HighLowItem(t, entry.High, entry.Low, entry.Open, entry.Close);
                ChartData.Add(item);
            }

            MarketChart = GetMarketChart();

            Title = (ChartData.Count > 0) ? "Loaded" : "Not Loaded";
            IsBusy = false;
        }


        //public async void LoadReport()
        //{
        //    IsBusy = true;
        //    var data = await AppData.Current.GetMarketHistory(Market);

        //    ReportHtml = GetReportHtml(data);
        //    IsBusy = false;
        //}

        public string GetReportHtml(List<MarketHistory> history)
        {
            var html = new StringBuilder();
            var inlineStyle = "style=\"width:95%;height:100%;\"";
            var inlineStyle1 = "style=\"width:100%;height:100%;\"";
            html.AppendLine("<html style=\"width:95%;height:100%;\">");
            html.AppendLine("<head>");
            html.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/apexcharts\"></script>");
            html.AppendLine("</head>");
            html.AppendLine($"<body {inlineStyle}>");
            html.AppendLine($"<div id=\"chart\" {inlineStyle1}>");

            html.AppendLine("</div>");
            html.AppendLine("<script>");

            var data = GetMarketData(history);

            html.AppendLine(@"  var options = {
                          chart: {
                            height: 300,
                            type: 'candlestick',
                            toolbar: {
                                show: true,
                                tools: {
                                    selection:true,
                                    zoom:true,
                                    zoomin:true,
                                    zoomout:true,
                                    download:false,
                                    pan:false,
                                    reset: true
                                }
                            },
                            
                            animations: {
                                enabled: false
                            }
                          },
                          series: [{           ");

            html.AppendLine($"data: {data}");
            html.AppendLine(@"
                           }],
                          title: {
                            text: 'CandleStick Chart',
                            align: 'left'
                          },
                          xaxis: {
                            type: 'datetime'
                          },
                          yaxis: {
                            tooltip: {
                              enabled: true
                            }
                          }
                        }");

            html.AppendLine(@"var chart = new ApexCharts(document.querySelector('#chart'), options); 
                            chart.render(); ");
            html.AppendLine("</script>");
            html.AppendLine("</body>");

            html.AppendLine("</html>");

            return html.ToString();
        }

        public bool IsLoading { get; set; }

        private string GetMarketData(List<MarketHistory> history)
        {
            string json = "[]";

            var entries = new List<ChartEntry>();
            var values = new List<string>();
            foreach (var data in history)
            {
                double t = DateTimeAxis.ToDouble(data.Date);
                var value = "{" + $"x:new Date({t}),y:[{data.Open},{data.High},{data.Low},{data.Close}]" + "}";
                values.Add(value);
                //var entry = new ChartEntry
                //{
                //    X = $"new Date({data.Timestamp})",
                //    Y = new[] { data.Open, data.High, data.Low, data.Close }
                //};

                //entries.Add(entry);
            }

            json = "[" + string.Join("," + Environment.NewLine, values) + "]";
            return json;



        }
        //public async void RefreshChartEntries()
        //{
        //    IsBusy = true;
        //    var history = await AppData.Current.GetMarketHistory(Market);
        //    var entries = new List<ChartEntry>();
        //    foreach (var data in history)
        //    {
        //        var val = (float)(data.High / data.Low) / 2;
        //        var entry = new ChartEntry(val)
        //        {
        //            Label = "",
        //            ValueLabel = $"{val}",
        //            Color = SKColor.Parse("#3498db")
        //        };

        //        entries.Add(entry);
        //    }
        //    var high = (float)history.Min(x => x.High);
        //    MarketChart = new LineChart()
        //    {
        //        Entries = entries,
        //        LineMode = LineMode.Straight,
        //        LineSize = 8,
        //        PointMode = PointMode.Square,
        //        PointSize = 18,

        //        MaxValue = high + (0.20f * high)
        //    };

        //    OnPropertyChanged(nameof(MarketChart));

        //    IsBusy = false;
        //}

        //public LineChart MarketChart { get; private set; }

        private PlotModel _marketChart;
        public PlotModel MarketChart
        {
            get
            {
                return _marketChart;
            }
            private set
            {
                _marketChart = value;
                OnPropertyChanged(nameof(MarketChart));
            }
        }

        public List<HighLowItem> ChartData { get; private set; }
        public PlotModel GetMarketChart()
        {

            var model = new PlotModel { Title = "LineSeries with default style" };
            //model.ResetAllAxes();
            model.InvalidatePlot(false);
            model.PlotAreaBorderColor = OxyColor.Parse("#B2BABB");
            model.TextColor = OxyColor.Parse("#B2BABB");
            model.LegendTextColor = OxyColor.Parse("#B2BABB");

            var dtAxis = new DateTimeAxis { Position = AxisPosition.Bottom };
            var laAxis = new LinearAxis { Position = AxisPosition.Left };

            model.Axes.Add(dtAxis);
            model.Axes.Add(laAxis);
            var items = ChartData.OrderBy(x=>x.X).ToArray();
            var series = new CandleStickSeries
            {
                Color = OxyColor.Parse("#B2BABB"),
                Selectable = true,
                Background = OxyColors.Transparent,
                SelectionMode = OxyPlot.SelectionMode.Single,
                IncreasingColor = OxyColor.Parse("#88FF88"),
                DecreasingColor = OxyColor.Parse("#FF5F5F"),
                TrackerFormatString =
                               "High: {2:0.00}\nLow: {3:0.00}\nOpen: {4:0.00}\nClose: {5:0.00}",
                ItemsSource = items
            };

            series.Title = Market.Pair;



            if (items.Length > 0)
            {
                dtAxis.Minimum = items[0].X;
                dtAxis.Maximum = items[items.Length - 1].X;

                laAxis.Minimum = items.Select(x => x.Low).Min();
                laAxis.Maximum = items.Select(x => x.High).Max();
            }
            model.Series.Add(series);
            model.InvalidatePlot(true);
            //foreach (var ax in plotView.Model.Axes)
            //    ax.Maximum = ax.Minimum = Double.NaN;
            //plotView.Model.InvalidatePlot()

            return model;

        }



        //private ObservableCollection<MarketOrder> _askList;
        //private ObservableCollection<MarketOrder> _bidList;
        //private ObservableCollection<Market> _tradeList;


        //public ObservableCollection<MarketOrder> AskList
        //{
        //    get => _askList;
        //    set
        //    {
        //        _askList = value;
        //        OnPropertyChanged(nameof(AskList));
        //    }
        //}

        //public ObservableCollection<MarketOrder> BidList
        //{
        //    get => _bidList;
        //    set
        //    {
        //        _bidList = value;
        //        OnPropertyChanged(nameof(BidList));
        //    }
        //}

        //private FormattedString _formattedPrice;
        //public FormattedString FormattedPrice
        //{
        //    get => _formattedPrice;
        //    set => SetProperty(ref _formattedPrice, value);
        //}

        //private FormattedString _formattedVolume;
        //public FormattedString FormattedVolume
        //{
        //    get => _formattedVolume;
        //    set => SetProperty(ref _formattedVolume, value);
        //}

        //private Market _marketDetail;
        //public Market MarketDetail
        //{
        //    get => _marketDetail;
        //    set => SetProperty(ref _marketDetail, value);
        //}



    }
}
