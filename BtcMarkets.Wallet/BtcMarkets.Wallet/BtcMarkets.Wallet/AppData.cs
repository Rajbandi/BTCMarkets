using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BtcMarkets.Core;
using BtcMarkets.Core.Api;
using BtcMarkets.Core.Helpers;
using BtcMarkets.Core.Sockets;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;

namespace BtcMarkets.Wallet
{

    public enum HistoryPeriod
    {
        Hour,
        Hour12,
        Day,
        Week,
        FortNight,
        Month,
        Quarter,
        HalfYear,
        Year,
        All
    }
    public class MarketEventArgs : EventArgs
    {
        public Market Market { get; set; }
    }

    public class AppData
    {

        private static AppData _data;

        private EventHandler<MarketEventArgs> _marketUpdated;

        public event EventHandler<MarketEventArgs> MarketUpdated
        {
            add
            {
                if (_marketUpdated == null || !_marketUpdated.GetInvocationList().Contains(value))
                {
                    _marketUpdated += value;
                }
            }
            remove
            {
                _marketUpdated -= value;
            }
        }

        private EventHandler _marketsUpdated;

        public event EventHandler MarketsUpdated
        {
            add
            {
                if (_marketsUpdated == null || !_marketsUpdated.GetInvocationList().Contains(value))
                {
                    _marketsUpdated += value;
                }
            }
            remove
            {
                _marketsUpdated -= value;
            }
        }

        private EventHandler _favouritesUpdated;

        public event EventHandler FavouritesUpdated
        {
            add
            {
                if (_favouritesUpdated == null || !_favouritesUpdated.GetInvocationList().Contains(value))
                {
                    _favouritesUpdated += value;
                }
            }
            remove
            {
                _favouritesUpdated -= value;
            }
        }
        public SocketClient SockClient { get; private set; }

        public AppSettings Settings { get; set; }
        public Models.ApiSettings ApiSettings { get; set; }

        public List<ActiveMarket> ActiveMarkets { get; private set; }

        public Market Market { get; set; }

        public List<Market> Markets { get; private set; }

        public List<Market> BtcMarkets => Markets?.Where(x => x.Currency == ApiConstants.Btc).ToList();
        public List<Market> AudMarkets => Markets?.Where(x => x.Currency == ApiConstants.Aud).ToList();

        public List<Market> Favourites => Markets?.Where(x => Settings.Favourites.Any(m => m.Instrument == x.Instrument && m.Currency == x.Currency)).ToList();

        public bool IsAccountSetup
        {
            get
            {
                var isSetup = false;

                var settings = Client?.Settings;
                if (settings != null)
                {
                    if (!string.IsNullOrWhiteSpace(settings.ApiKey) && !string.IsNullOrWhiteSpace(settings.Secret))
                    {
                        isSetup = true;
                    }
                }
                return isSetup;
            }
        }

        public bool IsAccountValid { get; private set; }

        public bool IsBusy { get; set; }
        protected AppData()
        {

            SockClient = new SocketClient();
            Markets = new List<Market>();

            LoadAppSettings();
        }

        public async void LoadAppSettings()
        {
            await Task.Factory.StartNew(() =>
            {
                if (Settings == null)
                {
                    var coinConfig = ResourceHelper.GetCoinConfig();

                    var settings = new AppSettings();
                    settings.Config = coinConfig;

                    Settings = settings;
                }
            });
        }


        protected virtual void OnMarketUpdated(MarketEventArgs e)
        {
            try
            {
                _marketUpdated?.Invoke(this, e);
            }
            catch(Exception ex)
            {
                AppHelper.TrackError(ex);
            }
        }
        protected virtual void OnMarketsUpdated(EventArgs e = null)
        {
            try
            {

                _marketsUpdated?.Invoke(this, e ?? EventArgs.Empty);
            }
            catch(Exception ex)
            {
                AppHelper.TrackError(ex);
            }
        }
        protected virtual void OnFavouritesUpdated(EventArgs e = null)
        {
            try
            {
                _favouritesUpdated?.Invoke(this, e ?? EventArgs.Empty);
            }
            catch(Exception ex)
            {
                AppHelper.TrackError(ex);
            }
        }
        public async Task LoadActiveMarkets()
        {

            var activeMarkets = await Api.GetActiveMarkets();

            var markets = new List<ActiveMarket>();
            foreach (var activeMarket in activeMarkets.Markets)
            {
                var market = new ActiveMarket
                {
                    Instrument = activeMarket.Instrument,
                    Currency = activeMarket.Currency
                };

                var coin = Settings?.Config?.Coinmarkets?.FirstOrDefault(x => x.Symbol == market.Instrument);
                if (coin != null)
                {
                    market.Name = coin.Name;
                    market.Image = coin.Image;
                }

                markets.Add(market);
            }

            ActiveMarkets = markets;

        }
        private readonly object _marketLock = new object();
        public void UpdateMarket(Market market)
        {
            lock (_marketLock)
            {

                var activeMarket = ActiveMarkets.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);

                var m = Markets.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
                if (m == null)
                {

                    if (string.IsNullOrWhiteSpace(market.Name))
                        market.Name = activeMarket.Name;

                    if (string.IsNullOrWhiteSpace(market.Image))
                        market.Image = activeMarket.Image;

                    market.Starred = Favourites.Any(x => x.Instrument == m.Instrument && x.Currency == m.Currency);
                    market.Notification = false;
                    Markets.Add(market);
                }
                else
                {
                    m.Ask = market.Ask;
                    m.Bid = market.Bid;
                    m.LastPrice = market.LastPrice;
                    m.Volume = market.Volume;

                    if (string.IsNullOrWhiteSpace(m.Name))
                        m.Name = activeMarket.Name;

                    if (string.IsNullOrWhiteSpace(m.Image))
                        m.Image = activeMarket.Image;

                    m.Starred = Favourites.Any(x => x.Instrument == m.Instrument && x.Currency == m.Currency);
                    m.Notification = false;
                }

                OnMarketUpdated(new MarketEventArgs
                {
                    Market = market
                });
            }
        }

        public void AddOrRemoveFavourite(Market market, bool remove = false)
        {
            if (market == null)
                return;

            var favourites = Settings.Favourites;

            var m = favourites.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
            if (remove)
            {
                if (m != null)
                {
                    favourites.Remove(m);
                    OnFavouritesUpdated(EventArgs.Empty);
                }
            }
            else
            {
                if (m == null)
                {
                    m = new MarketFavourite(market.Instrument, market.Currency);
                    favourites.Add(m);
                    OnFavouritesUpdated(EventArgs.Empty);
                }
            }

           
        }

        public async Task RefreshMarkets()
        {

            if (ActiveMarkets == null || !ActiveMarkets.Any())
            {
                await LoadActiveMarkets();
            }
            var markets = new List<Market>();
            Markets.Clear();
            foreach (var activeMarket in ActiveMarkets)
            {
                try
                {
                    var market = await Api.GetMarketTick(activeMarket.Instrument, activeMarket.Currency);

                    var m = Market.Load(market);

                    m.Name = activeMarket.Name;
                    m.Image = activeMarket.Image;

                    m.Starred = Favourites.Any(x => x.Instrument == m.Instrument && x.Currency == m.Currency);
                    m.Notification = false;

                    Markets.Add(m);
                }
                catch (Exception ex)
                {
                    AppHelper.TrackError(ex);
                }
            }

            OnMarketsUpdated(new EventArgs());

        }

        public async void CheckAndLoadActiveMarkets()
        {
            var activeMarkets = ActiveMarkets;
            if (activeMarkets == null || !activeMarkets.Any())
            {
                await LoadActiveMarkets();
            }
        }

        public async Task<Market> GetMarket(string instrument, string currency)
        {
            Market market = null;

            try
            {

                var data = await Api.GetMarketTick(instrument, currency);

                market = Market.Load(data);
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }

            return market;
        }

        public async Task<MarketTradeOrders> GetMarketTradeOrders(Market market)
        {
            var orders = new MarketTradeOrders();

            try
            {

                var orderBook = await Api.GetMarketOrderBook(market.Instrument, market.Currency);

                orders.Timestamp = orderBook.Timestamp;

                var sells = new List<MarketTradeOrder>();
                foreach (var value in orderBook.Asks)
                {
                    sells.Add(new MarketTradeOrder
                    {
                        Price = value[0],
                        Volume = value[1]
                    });
                }
                orders.Sell = sells;

                var buys = new List<MarketTradeOrder>();
                foreach (var value in orderBook.Bids)
                {
                    buys.Add(new MarketTradeOrder
                    {
                        Price = value[0],
                        Volume = value[1]
                    });
                }
                orders.Buy = buys;
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }

            return orders;
        }
        public async Task<List<MarketTradeHistory>> GetMarketTradeHistory(Market market)
        {
            var history = new List<MarketTradeHistory>();

            try
            {

                var tradeHistory = await Api.GetMarketTradesV2(market.Instrument, market.Currency);

                if (tradeHistory.Success)
                {
                    foreach (var value in tradeHistory.Trades)
                    {
                        var trade = new MarketTradeHistory
                        {
                            Price = ApiHelper.ToDoubleValue(value.Price.HasValue ? value.Price.Value : 0),
                            Volume = ApiHelper.ToDoubleValue(value.Volume.HasValue ? value.Volume.Value : 0),
                            CreationTime = value.CreationTime,
                            CreatedDate = ApiHelper.ToRelativeTime(value.CreationTime)
                        };

                        history.Add(trade);
                    }
                }
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }

            return history;
        }
        public async Task<List<MarketHistory>> GetMarketHistory(Market market, HistoryPeriod period = HistoryPeriod.Day)
        {
            var history = new List<MarketHistory>();

            DateTime startDate;

            var frequency = "hour";

            switch (period)
            {
                case HistoryPeriod.Hour:

                    startDate = DateTime.Now.AddHours(-1);
                    frequency = "minute";
                    break;
                case HistoryPeriod.Week:
                    startDate = DateTime.Now.AddDays(-7).Date;
                    frequency = "day";
                    break;
                case HistoryPeriod.FortNight:
                    startDate = DateTime.Now.AddDays(-15).Date;
                    frequency = "day";
                    break;
                case HistoryPeriod.Month:

                    startDate = DateTime.Now.AddMonths(-1).Date;
                    frequency = "day";
                    break;
                default:
                    startDate = DateTime.Today.AddDays(-1).Date;
                    break;
            }

            var timestamp = ApiHelper.GetTimeStampLong(startDate);

            var queryParams = new Core.Api.Contracts.MarketParams
            {
                Since = timestamp,
                IndexForward = true
            };

            var marketHistory = await Api.GetMarketHistory(market.Instrument, market.Currency, frequency, queryParams);

            if (marketHistory != null)
            {
                foreach (var tick in marketHistory.Ticks)
                {
                    var data = new MarketHistory
                    {
                        Instrument = market.Instrument,
                        Currency = market.Currency,
                        Date = ApiHelper.ToLocalTime(tick.Timestamp),
                        Timestamp = tick.Timestamp,
                        Open = ApiHelper.ToDoubleValue(tick.Open),
                        Close = ApiHelper.ToDoubleValue(tick.Close),
                        High = ApiHelper.ToDoubleValue(tick.High),
                        Low = ApiHelper.ToDoubleValue(tick.Low)
                    };

                    history.Add(data);
                }
            }

            return history;
        }

        public static AppData Current
        {
            get
            {
                lock (typeof(AppData))
                {
                    return _data ?? (_data = new AppData());
                }
            }
        }

        //private BtcMarketApi _api; 

        //public BtcMarketApi Api
        //{
        //    get
        //    {
        //        lock (typeof(BtcMarketApi))
        //        {
        //            if (_api == null)
        //            {
        //                var apiSettings = new Core.ApiSettings
        //                {
        //                    BaseUrl = "https://api.btcmarkets.net",
        //                    ApiKey = "c313c21c-3908-45ed-93e4-71c6b9f841ab",
        //                    Secret = "9poMsaAxsFskxhkL7IWCEaX5GKMxPPhippkshRjZ/dFzk33+4xMh/CTvgLaIK3UE6JpLQTpK/OqgxfF87DEeEg=="
        //                };

        //                _api = new BtcMarketApi(apiSettings);
        //            }

        //            return _api;
        //        }

        //    }
        //}

        public IBtcMarketsApi Api => Client.Api;

        private BtcMarketsClient _client;
        public BtcMarketsClient Client
        {
            get
            {
                lock (typeof(BtcMarketsClient))
                {
                    if (_client == null)
                    {
                        var apiSettings = new Core.ApiSettings
                        {
                            BaseUrl = "https://api.btcmarkets.net",
                            //      ApiKey = "c313c21c-3908-45ed-93e4-71c6b9f841ab",
                            //      Secret = "9poMsaAxsFskxhkL7IWCEaX5GKMxPPhippkshRjZ/dFzk33+4xMh/CTvgLaIK3UE6JpLQTpK/OqgxfF87DEeEg=="
                        };

                        _client = new BtcMarketsClient(apiSettings);
                    }
                    return _client;
                }
            }
        }
    }
}
