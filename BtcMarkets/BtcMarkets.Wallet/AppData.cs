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
using BtcMarkets.Wallet.Services;
using Newtonsoft.Json;

namespace BtcMarkets.Wallet
{

    public enum HistoryPeriod
    {
        Hour,
        Hour12,
        Day,
        HalfWeek,
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

        public List<AccountBalance> Balances { get; private set; }

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
            Balances = new List<AccountBalance>();

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
            catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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

            await LoadAccountBalances();

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

        public async Task LoadAccountBalances()
        {
            try
            {
                if (!IsAccountSetup)
                    return;

                var data = await Client.GetAccountBalance();
                if (data.Success.HasValue)
                {
                    var balances = data.Balances;

                    if (Markets != null)
                    {
                        Balances.Clear();

                        foreach (var balance in balances)
                        {

                            var bal = new AccountBalance
                            {
                                Balance = balance.Balance,
                                BalanceDecimal = balance.BalanceDecimal,
                                Currency = balance.Currency
                            };



                            var market = Markets.FirstOrDefault(x => x.Instrument == bal.Currency && x.Currency == Constants.Aud);
                            if (market != null)
                            {
                                market.Holdings = bal.BalanceDecimal;
                                bal.Name = market.Name;
                                bal.CurrencySymbol = market.InstrumentSymbol;
                                bal.Image = market.Image;
                                bal.TotalAud = bal.BalanceDecimal * market.LastPrice;
                            }
                            else
                            {

                                var coin = Settings?.Config?.Coinmarkets?.FirstOrDefault(x => x.Symbol == bal.Currency);
                                if (coin != null)
                                {
                                    bal.Name = coin.Name;
                                    if (bal.Currency == Constants.Aud)
                                    {
                                        bal.CurrencySymbol = Constants.AudSymbol;
                                    }
                                    else
                                        bal.CurrencySymbol = "  ";
                                    bal.Image = coin.Image;
                                    bal.TotalAud = bal.BalanceDecimal;
                                }
                            }
                            Balances.Add(bal);
                        }
                    }
                }
                else
                {
                    var error = "Something went wrong with balances.";
                    AppHelper.TrackError(new Exception($"{error}{data.ToJson()}"));
                    AppHelper.ShowError(error);
                }
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError("Something went wrong with balances.");
            }
        }

        public double TotalHoldingsInAud
        {
            get
            {
                double holdings = 0.0d;

                if (Markets != null)
                {
                    foreach (var balance in Balances)
                    {

                        var market = Markets.FirstOrDefault(x => x.Instrument == balance.Currency && x.Currency == Constants.Aud);
                        if (market != null)
                        {
                            holdings += balance.BalanceDecimal * market.LastPrice;
                        }
                        else
                        {
                            if (balance.Currency == Constants.Aud)
                            {
                                holdings += balance.BalanceDecimal;
                            }
                        }
                    }
                }

                return holdings;
            }
        }

        public double TotalHoldingsInBtc
        {
            get
            {
                double holdings = 0.0d;

                double audHoldings = TotalHoldingsInAud;

                if (Markets != null)
                {
                    var market = Markets.FirstOrDefault(x => x.Instrument == Constants.Btc && x.Currency == Constants.Aud);
                    if (market != null && audHoldings > 0)
                    {
                        holdings = audHoldings / market.LastPrice;
                    }
                }

                return holdings;
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
                AppHelper.ShowError("Something went wrong while retrieving trade history");
                AppHelper.TrackError(ex);
            }

            return history;
        }

        public async Task<List<MarketOrderData>> GetOpenOrders()
        {
            var orders = new List<MarketOrderData>();

            try
            {

                var response = await Api.GetOpenOrdersV2Raw();

                var openOrders = await Api.GetOpenOrdersV2();

                if (openOrders.Success)
                {
                    foreach (var value in openOrders.Orders)
                    {
                        var order = new MarketOrderData
                        {
                            Id = value.Id.HasValue?value.Id.Value:0,
                            Price = ApiHelper.ToDoubleValue(value.Price.HasValue ? value.Price.Value : 0),
                            Volume = ApiHelper.ToDoubleValue(value.Volume.HasValue ? value.Volume.Value : 0),
                            OpenVolume = ApiHelper.ToDoubleValue(value.OpenVolume.HasValue ? value.OpenVolume.Value : 0),
                            Timestamp = Convert.ToInt64(value.CreationTime),
                            Currency = value.Currency,
                            Instrument = value.Instrument,
                            OrderSide = value.OrderSide,
                            OrderType = value.Ordertype,
                            Status = value.Status
                        };

                        orders.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                AppHelper.ShowError("Something went wrong while retrieving open orders");
                AppHelper.TrackError(ex);
            }

            return orders;
        }

        public async Task<List<MarketOrderData>> GetOrderHistory(string instrument = Constants.Btc, string currency = Constants.Aud, long? since = null)
        {
            var orders = new List<MarketOrderData>();

            try
            {

                Core.Api.Contracts.MarketParams args = null;
                if (since.HasValue )
                {
                    args = new Core.Api.Contracts.MarketParams();
                    args.Since = since;
                    args.Forward = false;
                }

                var orderhistory = await Api.GetOrderHistoryV2(instrument, currency, args);

                if (orderhistory.Success)
                {
                    foreach (var value in orderhistory.Orders)
                    {
                        var order = new MarketOrderData
                        {
                            Id = value.Id.HasValue ? value.Id.Value : 0,
                            Price = ApiHelper.ToDoubleValue(value.Price.HasValue ? value.Price.Value : 0),
                            Volume = ApiHelper.ToDoubleValue(value.Volume.HasValue ? value.Volume.Value : 0),
                            OpenVolume = ApiHelper.ToDoubleValue(value.OpenVolume.HasValue ? value.OpenVolume.Value : 0),
                            Timestamp = Convert.ToInt64(value.CreationTime),
                            Currency = value.Currency,
                            Instrument = value.Instrument,
                            OrderSide = value.OrderSide,
                            OrderType = value.Ordertype,
                            Status = value.Status
                        };

                        orders.Add(order);
                    }
                }
                else
                {
                    AppHelper.ShowError();
                    AppHelper.TrackError(new Exception(JsonConvert.SerializeObject(orderhistory)));
                }
            }
            catch (Exception ex)
            {
                AppHelper.ShowError("Something went wrong while retrieving order history");
                AppHelper.TrackError(ex);
            }

            return orders;
        }

        public async Task<List<MarketHistory>> GetMarketHistory(Market market, HistoryPeriod period = HistoryPeriod.Day)
        {
            var history = new List<MarketHistory>();

            DateTime startDate;

            string frequency;

            switch (period)
            {
                case HistoryPeriod.Hour:

                    startDate = DateTime.Now.AddHours(-1);
                    frequency = "minute";
                    break;
                case HistoryPeriod.Hour12:
                    startDate = DateTime.Now.AddHours(-12);
                    frequency = "minute";
                    break;
                case HistoryPeriod.HalfWeek:
                    startDate = DateTime.Now.AddDays(-3).Date;
                    frequency = "hour";
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
                case HistoryPeriod.Quarter:

                    startDate = DateTime.Now.AddMonths(-3).Date;
                    frequency = "day";
                    break;
                case HistoryPeriod.HalfYear:

                    startDate = DateTime.Now.AddMonths(-6).Date;
                    frequency = "day";
                    break;
                case HistoryPeriod.Year:

                    startDate = DateTime.Now.AddYears(-1).Date;
                    frequency = "day";
                    break;
                default:
                    startDate = DateTime.Today.AddDays(-1).Date;
                    frequency = "hour";
                    break;
            }

            var timestamp = ApiHelper.GetTimeStampLong(startDate);

            var queryParams = new Core.Api.Contracts.MarketParams
            {
                Since = timestamp,
                Forward = true
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
                          
                        };

                        _client = new BtcMarketsClient(apiSettings);
                    }
                    return _client;
                }
            }
        }
    }
}
