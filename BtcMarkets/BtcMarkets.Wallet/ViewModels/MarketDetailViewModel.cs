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
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using BtcMarkets.Core.Helpers;
using BtcMarkets.Wallet.Controls;
using System.Windows.Input;

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

    public class ChartPeriod : BaseBindableObject
    {
        public ChartPeriod()
        {
            Style = "DefaultStackLayout";
        }

        public string Period { get; set; }
        public string Description { get; set; }

        private string _style;
        public string Style
        {
            get => _style;
            set => SetProperty(ref _style, value, nameof(Style));
        }
        public static HistoryPeriod GetPeriodValue(string periodStr)
        {

            HistoryPeriod period = HistoryPeriod.Day;
            switch (periodStr)
            {
                case "1H":
                    period = HistoryPeriod.Hour;
                    break;
                case "12H":
                    period = HistoryPeriod.Hour12;
                    break;
                case "1D":
                    period = HistoryPeriod.Day;
                    break;
                case "3D":
                    period = HistoryPeriod.HalfWeek;
                    break;
                case "1W":
                    period = HistoryPeriod.Week;
                    break;
                case "2W":
                    period = HistoryPeriod.FortNight;
                    break;
                case "1M":
                    period = HistoryPeriod.Month;
                    break;
                case "3M":
                    period = HistoryPeriod.Quarter;
                    break;
                case "6M":
                    period = HistoryPeriod.HalfYear;
                    break;
                case "1Y":
                    period = HistoryPeriod.Year;
                    break;

                default:
                    period = HistoryPeriod.Day;
                    break;
            }
            return period;
        }
    }

    public class MarketDetailViewModel : BaseViewModel
    {

        private Market _viewMarket;
        public Market ViewMarket
        {
            get;
            private set;
        }


        private Market _market;
        public Market Market
        {
            get => AppData.Current.Markets.FirstOrDefault(x=>x.Instrument == ViewMarket.Instrument && x.Currency == ViewMarket.Currency);
           
        }

        private string _low;
        public string Low
        {
            get => _low;
            set => SetProperty(ref _low, value, nameof(Low));
        }

        private string _high;
        public string High
        {
            get => _high;
            set => SetProperty(ref _high, value, nameof(High));
        } 

        private List<ChartPeriod> _chartPeriods;
        public List<ChartPeriod> ChartPeriods
        {
            get => _chartPeriods;
            private set => SetProperty(ref _chartPeriods, value, nameof(ChartPeriods));
        }

        private ChartPeriod _currentPeriod;
        public ChartPeriod CurrentPeriod
        {
            get => _currentPeriod;
            set => SetProperty(ref _currentPeriod, value, nameof(CurrentPeriod));
        }

        private ToggleImage _currentChart;
        public ToggleImage CurrentChart
        {
            get => _currentChart;
            set => SetProperty(ref _currentChart, value, nameof(CurrentChart));
        }

        private List<CandleStickEntry> _chartData;
        public List<CandleStickEntry> ChartData
        {
            get => _chartData;
            set
            {
                SetProperty(ref _chartData, value, nameof(ChartData));
                //Task.Run(() =>
                //{
                //    CandleChart = GetCandleChart();
                //});
                
            }
        }

        private List<DateValueEntry> _areaChartData;
        public List<DateValueEntry> AreaChartData
        {
            get => _areaChartData;
            set
            {
                SetProperty(ref _areaChartData, value, nameof(AreaChartData));
            }
        }

        private double? _areaChartPriceMinimum;
        public double? AreaChartPriceMinimum
        {
            get => _areaChartPriceMinimum;
            set => SetProperty(ref _areaChartPriceMinimum, value, nameof(AreaChartPriceMinimum));
        }

        private double? _areaChartPriceMaximum;
        public double? AreaChartPriceMaximum
        {
            get => _areaChartPriceMaximum;
            set => SetProperty(ref _areaChartPriceMaximum, value, nameof(AreaChartPriceMaximum));
        }

        private DateTime? _areaChartDateMinimum;
        public DateTime? AreaChartDateMinimum
        {
            get => _areaChartDateMinimum;
            set => SetProperty(ref _areaChartDateMinimum, value, nameof(AreaChartDateMinimum));
        }

        private DateTime? _areaChartDateMaximum;
        public DateTime? AreaChartDateMaximum
        {
            get => _areaChartDateMaximum;
            set => SetProperty(ref _areaChartDateMaximum, value, nameof(AreaChartDateMaximum));
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value, nameof(IsLoading));
        }

        public bool ViewLineChart => CurrentChart != null && !CurrentChart.Value;
        public bool ViewCandleChart => CurrentChart != null && CurrentChart.Value;

        public MarketDetailViewModel(Market market = null)
        {
            if (market == null)
            {
                market = AppData.Current.Market;
            }
            if (market != null)
            {
                ViewMarket = market;
                Title = $"{market.Name}";
            }
            // ChartData = new List<HighLowItem>();

            ChartData = new List<CandleStickEntry>();
            AreaChartData = new List<DateValueEntry>();

            ChartPeriods = new List<ChartPeriod>();
            LoadChartPeriods();
          
            CurrentChart = new ToggleImage
            {
                FontFamily = "MaterialDesign",
                Value = true,
                OnImage = "timeline",
                OffImage = "\uE85C",
                Color = "DefaultTextColor"
            };
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

        public void LoadChartPeriods()
        {
            if (ChartPeriods.Any())
                ChartPeriods.Clear();

            ChartPeriods.Add(new ChartPeriod { Period = "1D" });
            ChartPeriods.Add(new ChartPeriod { Period = "3D" });
            ChartPeriods.Add(new ChartPeriod { Period = "1W" });
            ChartPeriods.Add(new ChartPeriod { Period = "2W" });
            ChartPeriods.Add(new ChartPeriod { Period = "1M" });
            ChartPeriods.Add(new ChartPeriod { Period = "3M" });
            ChartPeriods.Add(new ChartPeriod { Period = "6M" });
            ChartPeriods.Add(new ChartPeriod { Period = "1Y" });
        }


        public  void InitReport()
        {
            if(CurrentPeriod == null)
            {
                CurrentPeriod = ChartPeriods.FirstOrDefault();
            }
            //if (CurrentPeriod != null)
            //{
            //    await LoadReport();
            //}
        }

        public async Task LoadReport(HistoryPeriod period = HistoryPeriod.Day)
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                try
                {
                    var data = await AppData.Current.GetMarketHistory(Market, period);
                    var dt = data.OrderBy(x => x.Date).ToList();

                    var chartData = new List<CandleStickEntry>();
                    foreach (var entry in dt)
                    {
                        chartData.Add(new CandleStickEntry(entry.Date, entry.Open, entry.High, entry.Low, entry.Close));
                    }

                    ChartData = chartData;

                    var areaChartData = chartData.Select(x => new DateValueEntry(x.Date, x.Close)).ToList();

                    AreaChartData = areaChartData;

                    var low = chartData.Min(x => x.Low);
                    var high = chartData.Max(x => x.High);

                    Low = $"{low:0.00}";
                    High = $"{high:0.00}";

                    AreaChartPriceMinimum = low;
                    AreaChartPriceMaximum = high;

                    AreaChartDateMinimum = chartData.Min(x => x.Date);
                    AreaChartDateMaximum = chartData.Max(x => x.Date);


                    await Task.Delay(100);
                }
                catch(Exception ex)
                {
                    AppHelper.TrackError(ex);
                    AppHelper.ShowError();
                }

                IsBusy = false;
            });
            });
        }
        public async Task<List<MarketHistory>> GetMarketHistory(HistoryPeriod period = HistoryPeriod.Day)
        {
            var data = await AppData.Current.GetMarketHistory(Market, period);
            var orderedHistory = data.OrderBy(x => x.Date).ToList();

            return orderedHistory;
        }
     
        public async void Refresh()
        {
            var current = CurrentPeriod;

            if (current != null)
            {
                var period = current.Period;
                var periodValue = ChartPeriod.GetPeriodValue(period);

                await LoadReport(periodValue);

                OnPropertyChanged(nameof(Market));
            }
        }
        public ICommand PeriodCommand
        {
            get
            {
                return new Command((value) =>
                {
                    Refresh();
                });

            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command((value) =>
                {
                    Refresh();
                });

            }
        }
        public ICommand ChangeChartCommand
        {
            get
            {
                return new Command(async (value) =>
                {
                    if (CurrentChart != null)
                    {
                        CurrentChart.Value = !CurrentChart.Value;
                        OnPropertyChanged(nameof(CurrentChart));
                        OnPropertyChanged(nameof(ViewLineChart));
                        OnPropertyChanged(nameof(ViewCandleChart));
                    }
                });

            }
        }

    }
}
