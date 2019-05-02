using BtcMarkets.Core.Helpers;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Controls
{
    public enum ChartType
    {
        Line,
        Bar,
        CandleStick,
        Depth
    }

    public interface IJsonEntry
    {
        string ToJson();
    }

    public abstract class DateEntry : IJsonEntry
    {
        public DateTime Date { get; set; }

        public long Timestamp
        {
            get
            {
                var dt = Date;
                var unixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                if (Date.Kind != DateTimeKind.Local)
                {
                    dt = DateTime.SpecifyKind(dt, DateTimeKind.Local);
                }

                var secondsSinceEpoch = (long)(dt - unixTime).TotalMilliseconds;
                return secondsSinceEpoch;
            }
        }

        public abstract string ToJson();

    }
    public class DateValueEntry : DateEntry
    {

        public DateValueEntry(DateTime dt, double value)
        {
            Date = dt;
            Value = value;
        }
        public double Value { get; set; }

        public override string ToJson()
        {
            return $"[{Timestamp},{Value}]";

        }
    }

    public class CandleStickEntry : DateEntry
    {

        public CandleStickEntry(DateTime dt, double open, double high, double low, double close)
        {
            Date = dt;
            High = high;
            Open = open;
            Low = low;
            Close = close;
        }

        public double High { get; set; }

        public double Low { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public override string ToJson()
        {
            return "{" + $"x:new Date({Timestamp}),y:[{Open},{High},{Low},{Close}]" + "}";
        }
    }

    public abstract class ChartView : WebView
    {
        public string ChartType { get; protected set; }
        public ChartView(string chartType)
        {

            ChartSource = new HtmlWebViewSource();
            ChartSource.BaseUrl = DependencyService.Get<IPlatformService>().GetHtmlBasePath();
            ChartSource.Html = ResourceHelper.GetChartHtml();

            this.Source = ChartSource;

            ChartType = chartType;
            this.Navigating += ChartView_Navigating;
            this.Navigated += ChartView_Navigated;

        }

        private void ChartView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            IsLoading = false;
        }

        private void ChartView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            IsLoading = true;
        }

        public TextAlignment TitleAlignment { get; set; }

        public bool? Animate { get; set; }

        public bool? Selection { get; set; }
        public bool? Zoom { get; set; }
        public bool? ShowToolbar { get; set; }

        public bool? ShowGridLines { get; set; }

        public bool? ShowGridXLines { get; set; }

        public bool? ShowGridYLines { get; set; }

        public bool? ShowTooltip { get; set; }

        public string Background { get; set; }

        public string ForeColor { get; set; }

        public abstract string GetData();

        public string ToOptions()
        {
            var options = new StringBuilder();
            options.Append("{");

            var json = new StringBuilder();


            json.Append($"type:'{ChartType}',");
            //if(this.Width>0)
            //{
            //    json.Append($"width:{Math.Round(Width)},");
            //}
            //if(this.Height>0)
            //{
            //    json.Append($"height:{Math.Round(Height)},");
            //}
            if (ShowToolbar.HasValue)
            {
                json.Append("toolbar: {");
                var show = ShowToolbar.Value ? "true" : "false";
                json.Append($"show:{show},");
                json.Append("},");
            }

            if (Animate.HasValue)
            {
                json.Append("animations: {");
                var show = Animate.Value ? "true" : "false";
                json.Append($"enabled:{show},");
                json.Append("},");
            }

            if (Selection.HasValue)
            {
                json.Append("selection: {");
                var show = Selection.Value ? "true" : "false";
                json.Append($"enabled:{show},");
                json.Append("},");
            }

            if (Zoom.HasValue)
            {
                json.Append("zoom: {");
                var show = Zoom.Value ? "true" : "false";
                json.Append($"enabled:{show},");
                json.Append("},");
                
            }

            if (!string.IsNullOrWhiteSpace(Background))
            {
                json.Append($"background: '{Background}', ");
            }

            if(!string.IsNullOrWhiteSpace(ForeColor))
            {
                json.Append($"forecolor: '{ForeColor}', ");
            }
            options.Append("chart:{");
            options.Append(json.ToString());
            options.Append("},");

            json.Clear();


            if (!string.IsNullOrWhiteSpace(Title))
            {
                json.Append($"text:{Title},");
                string align;
                switch (TitleAlignment)
                {
                    case TextAlignment.Center:
                        align = "center";
                        break;
                    case TextAlignment.End:
                        align = "right";
                        break;
                    default:
                        align = "left";
                        break;
                }
                json.Append($"align:{align}");

                options.Append("title:{");
                options.Append(json.ToString());
                options.Append("},");

                json.Clear();
            }

            if (ShowGridLines.HasValue && ShowGridLines.Value)
            {
                ShowGridXLines = true;
                ShowGridYLines = true;
            }

            if (ShowGridLines.HasValue)
            {
                var show = ShowGridLines.Value ? "true" : "false";
                json.Append($"show:{show},");
                json.Append("padding: {  left: 15, right: 0 }, ");
                if (ShowGridXLines.HasValue)
                {
                    show = ShowGridXLines.Value ? "true" : "false";
                    json.Append("xaxis:{");
                    json.Append("lines:{");
                    json.Append($"show:{show},");
                    json.Append("}");
                    json.Append("},");
                }
                if (ShowGridYLines.HasValue)
                {
                    show = ShowGridYLines.Value ? "true" : "false";
                    json.Append("yaxis:{");
                    json.Append("lines:{");
                    json.Append($"show:{show},");
                    json.Append("}");
                    json.Append("},");
                }
                options.Append("grid: {");
                options.Append(json.ToString());
                options.Append("},");

                json.Clear();
            }

            if (ShowTooltip.HasValue)
            {
                var show = ShowTooltip.Value ? "true" : "false";
                json.Append($"enabled:{show},");


                options.Append("tooltip: {");
                options.Append(json.ToString());
                options.Append("},");

                json.Clear();
            }

            options.Append("yaxis: {");
            options.Append("opposite:true");
            options.Append("},");

            json.Clear();


            options.Append(GetData());


            options.Append("}");
            return options.ToString();
        }


        public HtmlWebViewSource ChartSource { get; }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ChartView), default(string), BindingMode.OneWay);
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(ChartView), default(bool), BindingMode.TwoWay);
        public bool IsLoading
        {
            get
            {
                return (bool)GetValue(IsLoadingProperty);
            }
            set
            {
                SetValue(IsLoadingProperty, value);
            }
        }
        public abstract string GetOptions();

        private bool isFirstTime = true;
        public async Task Render()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    IsLoading = true;
                    var url = ChartSource.BaseUrl;
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        ChartSource.BaseUrl = DependencyService.Get<IPlatformService>().GetHtmlBasePath();
                    }

                    var options = GetOptions();
                    var callRender = $"update({options});";

                    //if (isFirstTime)
                    //{
                    //    isFirstTime = false;
                        //if (Device.RuntimePlatform == Device.iOS)
                        //{
                            await Task.Delay(100);
                        //}
                    //}
                    var result = await EvaluateJavaScriptAsync(callRender);
                    IsLoading = false;
                }
                catch (Exception ex)
                {
                    AppHelper.TrackError(ex);
                }

                var str = await GetHtmlLoaded();

            });
            });
        }

        public async Task<string> GetHtmlLoaded()
        {
            var result = await EvaluateJavaScriptAsync("return checkLoaded();");

            return result;
        }  
    }

    public class CandleChartView : ChartView
    {
        public CandleChartView() : base("candlestick")
        {
            ShowToolbar = false;
            Zoom = false;
            Selection = false;
            ShowTooltip = false;
            ShowGridLines = true;
            Animate = false;
        }

        public static readonly BindableProperty DataProperty = BindableProperty.Create(nameof(Data), typeof(IEnumerable<CandleStickEntry>), typeof(CandleChartView), default(IEnumerable<CandleStickEntry>), BindingMode.OneWay, null, propertyChanged: DataValueChanged);
        public IEnumerable<CandleStickEntry> Data
        {
            get
            {
                return (IEnumerable<CandleStickEntry>)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        private static void DataValueChanged(object source, object oldValue, object newValue)
        {
            var chart = (CandleChartView)source;
            if (chart != null)
            {
                if (oldValue != null)
                {
                    var data = (IEnumerable<CandleStickEntry>)newValue;

                    if (data != null)
                    {
                        Task.Run(async () =>
                        {
                            await chart.Render();
                        });
                    }
                }
            }
        }

        public override string GetData()
        {
            var bindingContext = BindingContext;
            var data = string.Empty;
            var entries = new List<string>();
            if (Data != null)
            {
                foreach (var entry in Data)
                {
                    entries.Add(entry.ToJson());
                }

                data = "[" + string.Join(",", entries) + "]";
            }

            var options = new StringBuilder();
            options.Append("series: [{");
            options.Append($"data:{data}");
            options.Append("}],");

            options.Append("xaxis:{ type:'datetime' },");


            return options.ToString();
        }

        public override string GetOptions()
        {
            return ToOptions();
        }
    }

    public class DateTimeAreaChartView : ChartView
    {
        public DateTimeAreaChartView() : base("area")
        {
            ShowToolbar = false;
            Zoom = false;
            Selection = false;
            ShowTooltip = false;
            ShowGridLines = true;
            Animate = false;
        }

        public static readonly BindableProperty DataProperty = BindableProperty.Create(nameof(Data), typeof(IEnumerable<DateValueEntry>), typeof(DateTimeAreaChartView), default(IEnumerable<DateValueEntry>), BindingMode.OneWay, null, propertyChanged: DataValueChanged);
        public IEnumerable<DateValueEntry> Data
        {
            get
            {
                return (IEnumerable<DateValueEntry>)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        private static void DataValueChanged(object source, object oldValue, object newValue)
        {
            var chart = (DateTimeAreaChartView)source;
            if (chart != null)
            {
                if (oldValue != null)
                {
                    var data = (IEnumerable<DateValueEntry>)newValue;

                    if (data != null)
                    {
                        Task.Run(async () =>
                        {
                            await chart.Render();
                        });
                    }
                }
            }
        }

        public override string GetData()
        {

            var data = string.Empty;
            var entries = new List<string>();
            if (Data != null)
            {
                foreach (var entry in Data)
                {
                    entries.Add(entry.ToJson());
                }

                data = "[" + string.Join(",", entries) + "]";
            }

            var options = new StringBuilder();
            options.Append("series: [{");
            options.Append($"data:{data}");
            options.Append("}],");

            options.Append("xaxis:{ type:'datetime' },");
            options.Append("dataLabels:{ enabled: false},");

            return options.ToString();
        }

        public override string GetOptions()
        {
            return ToOptions();
        }
    }
}
