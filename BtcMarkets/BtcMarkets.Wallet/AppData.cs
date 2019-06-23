using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using BtcMarkets.Core;
using BtcMarkets.Core.Api;
using BtcMarkets.Core.Helpers;
using BtcMarkets.Core.Sockets;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.Services;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

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

    public class AppData : BaseBindableObject
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
                if (_marketUpdated != null && _marketUpdated.GetInvocationList().Contains(value))
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
                if (_marketsUpdated != null && _marketsUpdated.GetInvocationList().Contains(value))
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
                if (_favouritesUpdated != null && _favouritesUpdated.GetInvocationList().Contains(value))
                    _favouritesUpdated -= value;
            }
        }
        public SocketClient SockClient { get; private set; }
        public SocketV2Client SockV2Client { get; private set; }
        public AppSettings Settings { get; set; }
        public Models.ApiSettings ApiSettings { get; set; }

        public List<ActiveMarket> ActiveMarkets { get; private set; }

        public Market Market { get; set; }

        private List<Market> _markets;
        public List<Market> Markets
        {
            get => _markets;
            private set
            {
                SetProperty(ref _markets, value, nameof(Markets));

                LoadMarkets();
            }
        }

        public List<AccountBalance> Balances { get; private set; }

        private List<Market> _btcMarkets;
        public List<Market> BtcMarkets
        {
            get => _btcMarkets;
            set
            {
                SetProperty(ref _btcMarkets, value, nameof(BtcMarkets));
            }
        }

        private List<Market> _audMarkets;
        public List<Market> AudMarkets
        {
            get => _audMarkets;
            set
            {

                SetProperty(ref _audMarkets, value, nameof(AudMarkets));
            }
        }

        private List<Market> _favourites;
        public List<Market> Favourites
        {
            get
            {

                return new List<Market>(Markets?.Where(x => Settings.Favourites.Any(m => m.Instrument == x.Instrument && m.Currency == x.Currency)).ToList());
            }
        }

        private bool _liveDataAvailable;
        public bool LiveDataAvailable
        {
            get => _liveDataAvailable;
            set
            {
                SetProperty(ref _liveDataAvailable, value, nameof(LiveDataAvailable));
            }
        }

        private AccountValue _accountValue;
        public AccountValue AccountHoldings
        {
            get => _accountValue;
            set => SetProperty(ref _accountValue, value, nameof(AccountHoldings));
        }

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

        private bool _isAccountValid;
        public bool IsAccountValid
        {
            get => _isAccountValid;
            set
            {
                SetProperty(ref _isAccountValid, value, nameof(IsAccountValid));
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value);
                try
                {
                    if (value)
                    {
                        AppService.Instance.ShowLoader();
                    }
                    else
                    {
                        AppService.Instance.HideLoader();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }


        public bool IsInternetConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        protected AppData()
        {


            CheckInternet();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            Markets = new List<Market>();
            Balances = new List<AccountBalance>();


            InitData();

        }

        public async void InitData()
        {
            await LoadAppSettings();


        }
        public async Task InitMarkets()
        {

            await RefreshMarkets();

        }

        public void CheckFavourites()
        {
            foreach (var market in Markets)
            {
                market.Starred = Favourites.Any(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
            }
        }
        private void LoadMarkets()
        {
            if (AudMarkets == null)
            {
                AudMarkets = new List<Market>();
            }
            else
            {
                AudMarkets.Clear();
            }
            if (BtcMarkets == null)
            {
                BtcMarkets = new List<Market>();
            }
            else
            {
                BtcMarkets.Clear();
            }
            if (Markets != null)
            {
                foreach (var market in Markets.Where(x => x.Currency == ApiConstants.Aud))
                {
                    AudMarkets.Add(market);
                }
                foreach (var market in Markets.Where(x => x.Currency == ApiConstants.Btc))
                {
                    BtcMarkets.Add(market);
                }

                OnPropertyChanged(nameof(AudMarkets));
                OnPropertyChanged(nameof(BtcMarkets));
            }

        }

        public async Task InitSocketsV2(bool subscribe = true)
        {
            try
            {
                if (!CheckSockets())
                {
                    return;
                }
                if (SockV2Client == null)
                {
                    SockV2Client = new SocketV2Client(new SocketOptions
                    {
                        Url = Client.Settings.SocketV2Url
                    });


                    SockV2Client.Opened += SockV2Client_Opened;
                    SockV2Client.Closed += SockV2Client_Closed;
                    SockV2Client.FundTransferred += SockV2Client_FundTransferred;
                    SockV2Client.MarketTradeChanged += SockV2Client_MarketTradeChanged;
                    SockV2Client.SendFailed += SockV2Client_SendFailed;
                    SockV2Client.StateChanged += SockV2Client_StateChanged;
                    SockV2Client.TickerChanged += SockV2Client_TickerChanged;
                    SockV2Client.OrderBookChanged += SockV2Client_OrderBookChanged;
                }
                if (!SockV2Client.IsOpen)
                {
                    await SockV2Client.Open();
                    await SubscibedTicker();
                }
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError("Live updates not available");
            }
        }

        public void CloseSocketsV2()
        {
            try
            {
                if (SockV2Client != null && SockV2Client.IsOpen)
                {
                    SockV2Client.Close();
                }
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }
        }

        public async void OpenCloseSocketsV2(bool value)
        {
            string message = "";
            try
            {
               
                if (value)
                {
                    if (SockV2Client != null && SockV2Client.IsOpen)
                        return;

                    await InitSocketsV2();
                    
                    message = "Live updates enabled.";
                }
                else
                {
                    CloseSocketsV2();
                    message = "Live updates disabled.";
                }
            }
            catch(Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError("Something went wrong.");

            }
            if(!string.IsNullOrWhiteSpace(message))
            {
                AppHelper.ShowMessage(message);
            }
        }

        public void ChangeTheme(string theme)
        {
            var app = (App)Application.Current;
            if (theme == Constants.ThemeLight)
            {
                app.ChangeTheme(ThemeList.LightTheme);
            }
            else
                app.ChangeTheme(ThemeList.DarkTheme);
        }
        private async Task SubscibedTicker()
        {
            try
            {
                if (SockV2Client == null || !SockV2Client.IsOpen)
                    return;

                var message = new SubscribeMessage();
                message.Channels = new List<string>
                {
                    ChannelType.Tick
                };
                var marketIds = ActiveMarkets.Select(x => x.Id).ToList();
                if (!marketIds.Any())
                {
                    marketIds = new List<string> { "BTC-AUD" };
                }
                message.MarketIds = marketIds;

                await SockV2Client.Subscribe(message);
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError("Live ticker not available");
            }
        }

        private void SockV2Client_OrderBookChanged(object sender, OrderBookDataV2 e)
        {

        }

        private void SockV2Client_TickerChanged(object sender, TickerDataV2 e)
        {
            Task.Run(() =>
            {
                var marketPair = e.MarketId;
                var market = Markets.FirstOrDefault(x => x.Id == marketPair);
                if (market == null)
                {
                    market = new Market();
                }

                if (market.Bid != e.BestBid)
                    market.Bid = e.BestBid;

                if (market.Ask != e.BestAsk)
                    market.Ask = e.BestAsk;

                if (e.Volume24h > 0 && market.Volume != e.Volume24h)
                    market.Volume = e.Volume24h;

                if (market.LastPrice != e.LastPrice)
                {
                    market.PreviousPrice = market.LastPrice;
                    market.LastPrice = e.LastPrice;
                }

                UpdateHoldings();

                UpdateAccountBalance(market);


                OnMarketUpdated(new MarketEventArgs
                {
                    Market = market
                });
            });
            //UpdateMarket(market);
        }

        private void SockV2Client_StateChanged(object sender, SocketState e)
        {

        }

        private void SockV2Client_SendFailed(object sender, EventArgs e)
        {

        }

        private void SockV2Client_MarketTradeChanged(object sender, MarketTradeDataV2 e)
        {

        }

        private void SockV2Client_FundTransferred(object sender, FundTransferDataV2 e)
        {

        }

        private void SockV2Client_Closed(object sender, EventArgs e)
        {
            LiveDataAvailable = false;
        }

        private void SockV2Client_Opened(object sender, EventArgs e)
        {
            LiveDataAvailable = true;
        }

        private void InitSockets(Action action = null)
        {
            try
            {
                if (!CheckSockets())
                {
                    return;
                }
                SockClient = new SocketClient(new SocketOptions
                {
                    Url = Client.Settings.SocketUrl
                });

                SockClient.Connected += SockClient_Connected;
                SockClient.Disconnected += SockClient_Disconnected;

                SockClient.TickerChanged += SockClient_TickerChanged;
                SockClient.MarketTradeChanged += SockClient_MarketTradeChanged;
                SockClient.OrderBookChanged += SockClient_OrderBookChanged;

                SockClient.Open(() =>
                {
                    if (action != null)
                    {
                        action.Invoke();
                    }

                });
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError("Live updates not available");
            }
        }


        private void SockClient_Disconnected(object sender, SocketEventArgs e)
        {
            LiveDataAvailable = false;
        }

        private void SockClient_Connected(object sender, SocketEventArgs e)
        {
            LiveDataAvailable = true;
        }

        private void CloseSockets()
        {
            try
            {
                if (SockClient == null)
                    return;

                if (!CheckSockets())
                {
                    return;
                }

                SockClient.TickerChanged -= SockClient_TickerChanged;
                SockClient.MarketTradeChanged -= SockClient_MarketTradeChanged;
                SockClient.OrderBookChanged -= SockClient_OrderBookChanged;

                SockClient.Close();
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }
        }

        private void SockClient_TickerChanged(object sender, TickerEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(e.Error))
                {
                    var tick = e.Ticker;


                    var market = new Market
                    {
                        Instrument = tick.Instrument,
                        Currency = tick.Currency,
                        Ask = tick.BestAskDouble,
                        Bid = tick.BestBidDouble,
                        Volume = tick.Volume24hDouble,
                        LastPrice = tick.LastPriceDouble
                    };
                    UpdateMarket(market);

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void SockClient_MarketTradeChanged(object sender, MarketTradeEventArgs e)
        {

        }

        private void SockClient_OrderBookChanged(object sender, OrderBookEventArgs e)
        {

        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var connected = e.NetworkAccess == NetworkAccess.Internet;
            if (!connected)
            {
                AppHelper.ShowError("Check your internet connection");
            }
            else
            {
                await CheckAndInitSockets();
            }

        }

        public void OpenCloseSockets(bool close = false)
        {
            if (close)
            {
                LeaveMarketChannels();
                CloseSockets();
            }
            else
            {
                var open = CheckSocketsAvailable();
                if (!open)
                {
                    AppHelper.ShowError("Live data not available.");
                }
                JoinMarketChannels();
            }
        }
        private bool CheckSocketsAvailable()
        {
            return (SockClient != null && SockClient.IsOpen && LiveDataAvailable);

        }
        private bool _marketSockets;

        public void JoinMarketChannels(bool force = false)
        {
            try
            {

                LeaveMarketChannels(false);

                if (!_marketSockets && ActiveMarkets.Any())
                {
                    var socketMarkets = ActiveMarkets.Select(x => new SocketMarket
                    {
                        Instrument = x.Instrument,
                        Currency = x.Currency,
                        Ticker = true
                    }).ToList();

                    SockClient.JoinChannels(socketMarkets);
                    _marketSockets = true;
                }
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError("Live market data not available");

            }
        }

        public void LeaveMarketChannels(bool main = true)
        {
            try
            {
                if (!SockClient.IsOpen)
                    return;

                if (_marketSockets)
                {
                    var socketMarkets = ActiveMarkets.Select(x => new SocketMarket
                    {
                        Instrument = x.Instrument,
                        Currency = x.Currency,
                        Ticker = false
                    }).ToList();
                    SockClient.LeaveChannels(socketMarkets);
                    _marketSockets = false;
                }
            }
            catch (Exception ex)
            {
                if (main)
                {
                    AppHelper.TrackError(ex);
                }
            }
        }

        public bool CheckReachbility(string url)
        {
            try
            {
                var uri = new Uri(url);
                var host = uri.Host;
                var port = uri.Port;
                using (var client = new TcpClient(host, port))
                    return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public bool CheckInternet()
        {
            bool canReach = true;

            if (!IsInternetConnected)
            {
                AppHelper.ShowError("Check your internet connection");
                canReach = false;
            }


            if (IsInternetConnected)
            {
                canReach = CheckReachbility(Client.Settings.BaseUrl);
                if (!canReach)
                {
                    AppHelper.ShowError("Couldn't reach server. Check your connection");
                }
            }

            return IsInternetConnected && canReach;
        }
        public async Task CheckAndInitSockets()
        {
            if (Settings.LiveUpdates)
            {
                if (SockV2Client == null || !SockV2Client.IsOpen)
                {
                    await InitSocketsV2();
                    //await SubscibedTicker();
                }
            }
            else
            {
                CloseSocketsV2();
            }
        }
        public bool CheckSockets()
        {
            bool canReach = true;

            if (!IsInternetConnected)
            {
                AppHelper.ShowError("Check your internet connection");
                canReach = false;
            }


            if (IsInternetConnected)
            {
                canReach = CheckReachbility(Client.Settings.SocketUrl);
                if (!canReach)
                {
                    AppHelper.ShowError("Couldn't reach live server. Check your connection");
                }
            }

            return IsInternetConnected && canReach;
        }
        public bool CheckSocketsV2()
        {
            bool canReach = true;

            if (!IsInternetConnected)
            {
                AppHelper.ShowError("Check your internet connection");
                canReach = false;
            }

            if (IsInternetConnected)
            {
                canReach = CheckReachbility(Client.Settings.SocketV2Url);
                if (!canReach)
                {
                    AppHelper.ShowError("Couldn't reach live server. Check your connection");
                }
            }

            return IsInternetConnected && canReach;
        }
        public async Task LoadAppSettings()
        {
            await Task.Run(async () =>
           {
               isBusy = true;
               var save = false;
               LoadSettings();

               if (Settings == null)
               {
                   Settings = new AppSettings();
                   save = true;
               }
               if (Settings.Config == null)
               {

                   Settings.Config = ResourceHelper.GetCoinConfig();
               }

               var remoteConfig = await GetRemoteConfig();
               if (remoteConfig != null)
               {
                   var localConfig = Settings.Config;
                   if (remoteConfig.Version != localConfig.Version || remoteConfig.Lastmodified > localConfig.Lastmodified)
                   {
                       Settings.Config = remoteConfig;
                       save = true;
                   }
               }

               if (save)
               {
                   SaveSettings();
               }

               var crendentials = Settings.ApiCredentials;
               if (crendentials != null)
               {
                   Client.Settings.ApiKey = crendentials.ApiKey;
                   Client.Settings.Secret = crendentials.Secret;
               }

               IsAccountValid = IsAccountSetup;

               LoadMarketImages();
               isBusy = false;
           });
        }

        private string _configUrl = "http://btcmarketsapp.s3-website-ap-southeast-2.amazonaws.com/settings.json";
        public async Task<CoinConfig> GetRemoteConfig()
        {
            CoinConfig config = null;
            try
            {
                var client = new HttpClient();

                var json = await client.GetStringAsync(_configUrl);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    config = JsonConvert.DeserializeObject<CoinConfig>(json);
                }
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }

            return config;
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

            if (!CheckInternet())
            {
                return;
            }

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

                    var bal = Balances.FirstOrDefault(x => x.Currency == market.Instrument && market.Currency == Constants.Aud);
                    if (bal != null)
                    {
                        market.Holdings = bal.BalanceDecimal;
                    }

                    Markets.Add(market);

                }
                else
                {
                    m.Ask = market.Ask;
                    m.Bid = market.Bid;

                    m.PreviousPrice = m.LastPrice;
                    market.PreviousPrice = m.LastPrice;

                    m.LastPrice = market.LastPrice;

                    if (m.Volume != market.Volume && market.Volume > 0)
                        m.Volume = market.Volume;

                    m.Change = AppHelper.CalculateChange(m.PreviousPrice, m.LastPrice);

                    if (string.IsNullOrWhiteSpace(m.Name))
                        m.Name = activeMarket.Name;

                    if (string.IsNullOrWhiteSpace(m.Image))
                        m.Image = activeMarket.Image;

                    m.Starred = Favourites.Any(x => x.Instrument == m.Instrument && x.Currency == m.Currency);
                    m.Notification = false;

                    var bal = Balances.FirstOrDefault(x => x.Currency == m.Instrument && m.Currency == Constants.Aud);
                    if (bal != null)
                    {
                        market.Holdings = bal.BalanceDecimal;
                    }

                }

                UpdateHoldings();

                UpdateAccountBalance(market);

                OnMarketUpdated(new MarketEventArgs
                {
                    Market = market
                });

            }
        }

        public void UpdateAccountBalance(Market market)
        {
            if (AccountHoldings == null || market == null)
                return;

            var balances = AccountHoldings.Balances;
            if (balances == null)
                return;

            var balance = balances.FirstOrDefault(x => x.Currency == market.Instrument);
            if (balance != null)
            {
                balance.TotalAud = balance.BalanceDecimal * market.LastPrice;
            }

        }

        public void UpdateAccountBalances()
        {
            if (AccountHoldings == null)
            {
                AccountHoldings = new AccountValue();
            }

            var accountBalances = new List<AccountBalance>();

            var balances = Balances.Where(x => x.Balance > 0).OrderBy(x => x.Currency);
            var audBalance = balances.FirstOrDefault(x => x.Currency == Constants.Aud);
            if (audBalance != null)
            {
                accountBalances.Add(audBalance);
            }
            var btcBalance = balances.FirstOrDefault(x => x.Currency == Constants.Btc);
            if (btcBalance != null)
            {
                accountBalances.Add(btcBalance);
            }
            foreach (var balance in balances.Where(x => x.Currency != Constants.Aud && x.Currency != Constants.Btc))
            {
                accountBalances.Add(balance);
            }

            AccountHoldings.Balances = new ObservableCollection<AccountBalance>(accountBalances);
        }
        public void UpdateHoldings()
        {
            if (AccountHoldings == null)
            {
                AccountHoldings = new AccountValue();
            }

            AccountHoldings.BtcAudMarket = Markets.FirstOrDefault(x => x.Instrument == Constants.Btc && x.Currency == Constants.Aud);
            AccountHoldings.AccountValueInAud = TotalHoldingsInAud;
            AccountHoldings.AccountValueInBtc = TotalHoldingsInBtc;



            OnPropertyChanged(nameof(AccountHoldings));
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
                    SaveSettings();
                }
            }
            else
            {
                if (m == null)
                {
                    m = new MarketFavourite(market.Instrument, market.Currency);
                    favourites.Add(m);
                    OnFavouritesUpdated(EventArgs.Empty);
                    SaveSettings();
                }
            }


        }

        public async Task RefreshMarkets()
        {
            if (!CheckInternet())
            {
                return;
            }


            if (ActiveMarkets == null || !ActiveMarkets.Any())
            {
                await LoadActiveMarkets();
            }
            var markets = new List<Market>();


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

                    markets.Add(m);
                }
                catch (Exception ex)
                {
                    AppHelper.TrackError(ex);
                }
            }

            Markets = markets;

            CheckFavourites();

            OnPropertyChanged(nameof(Markets));
            OnMarketsUpdated(new EventArgs());

            await CheckAndInitSockets();

            LoadBalances();

        }

        public void LoadBalances()
        {
            Task.Run(async () =>
            {
                await Task.Delay(2000);
                await LoadAccountBalances();
                await LoadPreviousDayPrices();
                UpdateHoldings();
                UpdateAccountBalances();
                OnPropertyChanged(nameof(Markets));

                OnMarketsUpdated(new EventArgs());
            });
        }
        public async Task LoadPreviousDayPrices()
        {
            try
            {
                if(Markets != null)
                {
                    foreach(var market in Markets)
                    {
                        var history = await GetMarketDayHistory(market);
                        if(history != null)
                        {
                            var previousPrice = history.Open;
                            market.PreviousDayPrice = previousPrice;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError("24H history prices not available");
            }
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
                if (!CheckInternet())
                {
                    return;
                }

                if (!IsAccountValid)
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

            if (!CheckInternet())
            {
                return market;
            }
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

            if (!CheckInternet())
            {
                return orders;
            }
            try
            {

                var orderBook = await Api.GetMarketOrderBook(market.Instrument, market.Currency);

                orders.Timestamp = orderBook.Timestamp;

                var sells = new List<MarketTradeOrder>();
                foreach (var value in orderBook.Asks)
                {
                    double total = value[0] * value[1];
                    sells.Add(new MarketTradeOrder
                    {
                        Price = value[0],
                        Volume = value[1],
                        Total = total
                    });
                }
                orders.Sell = sells;

                var buys = new List<MarketTradeOrder>();
                foreach (var value in orderBook.Bids)
                {
                    double total = value[0] * value[1];
                    buys.Add(new MarketTradeOrder
                    {
                        Price = value[0],
                        Volume = value[1],
                        Total = total
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

            if (!CheckInternet())
            {
                return history;
            }
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
            if (!CheckInternet())
            {
                return orders;
            }
            if (!IsAccountSetup)
                return orders;
            try
            {
                var openOrders = await Api.GetOpenOrdersV2();

                if (openOrders.Success)
                {
                    foreach (var value in openOrders.Orders)
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
            }
            catch (Exception ex)
            {
                AppHelper.ShowError("Something went wrong while retrieving open orders");
                AppHelper.TrackError(ex);
            }


            return orders;
        }

        public async Task<List<FundTransferData>> GetFundTransferHistory(long? timestamp = null)
        {
            var funds = new List<FundTransferData>();
            if (!CheckInternet())
            {
                return funds;
            }
            if (!IsAccountSetup)
                return funds;
            try
            {
                var fundHistory = await Api.GetFundTransferHistory();

                if (fundHistory.Success)
                {
                    foreach (var value in fundHistory.FundTransfers)
                    {
                        var fund = new FundTransferData
                        {
                            Id = value.FundTransferId,
                            Amount = ApiHelper.ToDoubleValue(value.Amount.HasValue ? value.Amount.Value: 0),
                            Fee = ApiHelper.ToDoubleValue(value.Fee.HasValue ? value.Fee.Value : 0),
                            Currency = value.Currency,
                            Status = value.Status,
                            Description = value.Description,
                            Timestamp = value.CreationTime,
                            LastUpdate = value.LastUpdate
                        };

                        var detail = value.CryptoPaymentDetail;
                        if(detail != null)
                        {
                            fund.TxId = detail.TxId;
                            fund.Address = detail.Address;
                        }
                        funds.Add(fund);
                    }
                }
            }
            catch (Exception ex)
            {
                AppHelper.ShowError("Something went wrong while retrieving fund history");
                AppHelper.TrackError(ex);
            }


            return funds;
        }

        public async Task<List<MarketOrderData>> GetOrderHistory(string instrument = Constants.Btc, string currency = Constants.Aud, long? since = null)
        {
            var orders = new List<MarketOrderData>();

            if (!CheckInternet())
            {
                return orders;
            }
            if (!IsAccountSetup)
                return orders;

            try
            {

                Core.Api.Contracts.MarketParams args = null;
                if (since.HasValue)
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

        public async Task<MarketHistory> GetMarketDayHistory(Market market)
        {
            MarketHistory history = null;

            if (!CheckInternet())
            {
                return history;
            }
            try
            {
                DateTime startDate = DateTime.Now.AddDays(-1);
                var timestamp = ApiHelper.GetTimeStampLong(startDate);
                var queryParams = new Core.Api.Contracts.MarketParams
                {
                    Since = timestamp,
                    Forward = true
                };
                var marketHistory = await Api.GetMarketHistory(market.Instrument, market.Currency, "day", queryParams);

                if (marketHistory != null)
                {
                    var ticks = marketHistory.Ticks;
                    if (ticks != null && ticks.Any())
                    {
                        var tick = ticks[0];
                        history = new MarketHistory
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
                    }
                }
            }
            catch(Exception ex)
            {
                AppHelper.TrackError(ex);
            }
            return history;
        }

        public async Task<List<MarketHistory>> GetMarketHistory(Market market, HistoryPeriod period = HistoryPeriod.Day)
        {
            var history = new List<MarketHistory>();

            if (!CheckInternet())
            {
                return history;
            }
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
                    frequency = "hour";
                    break;
                case HistoryPeriod.FortNight:
                    startDate = DateTime.Now.AddDays(-15).Date;
                    frequency = "hour";
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

        private readonly string _settingsKey = "settings";
        private readonly string _credentialsKey = "credentials";
        public async void LoadSettings()
        {
            try
            {
                var json = Preferences.Get(_settingsKey, "");
                if (!string.IsNullOrEmpty(json))
                {
                    Settings = JsonConvert.DeserializeObject<AppSettings>(json);

                    if (Settings != null)
                    {
                        var apiJson = await SecureStorage.GetAsync(_credentialsKey);
                        if (!string.IsNullOrWhiteSpace(apiJson))
                        {
                            var credentials = JsonConvert.DeserializeObject<ApiCredentials>(apiJson);
                            if (credentials != null && Settings != null)
                            {
                                Settings.ApiCredentials = credentials;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
            }
        }

        public async void SaveSettings(bool show = false)
        {
            try
            {

                var json = JsonConvert.SerializeObject(Settings);
                Preferences.Set(_settingsKey, json);

                var credentials = Settings.ApiCredentials;

                if (credentials != null)
                {
                    var apiKey = credentials.ApiKey;
                    var secret = credentials.Secret;
                    var clientSettings = Client.Settings;

                    if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(secret)
                        && apiKey != clientSettings.ApiKey && secret != clientSettings.Secret)
                    {
                        clientSettings.ApiKey = apiKey;
                        clientSettings.Secret = secret;
                        IsAccountValid = IsAccountSetup;

                        var apiJson = JsonConvert.SerializeObject(credentials);
                        await SecureStorage.SetAsync(_credentialsKey, apiJson);
                    }
                }

                if (show)
                    AppHelper.ShowMessage("Settings saved successfully.");

            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
                if (show)
                    AppHelper.ShowError("Something went wrong. Can't save settings");
            }
        }

        private Dictionary<string, ImageSource> _marketImages;

        public Dictionary<string, ImageSource> MarketImages { get; set; }

        private void LoadMarketImages()
        {
            if (MarketImages == null)
            {
                MarketImages = new Dictionary<string, ImageSource>();
            }
            var config = Settings?.Config;
            if (config != null)
            {
                foreach (var market in config.Coinmarkets)
                {
                    var image = market.Image;
                    var code = market.Symbol;

                    if (!string.IsNullOrWhiteSpace(image))
                    {
                        if (!MarketImages.ContainsKey(code))
                        {

                            byte[] bytes = System.Convert.FromBase64String(image);
                            var source = ImageSource.FromStream(() => new MemoryStream(bytes));
                            MarketImages.Add(code, source);
                        }
                    }
                }
            }
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
                            SocketUrl = "https://socket.btcmarkets.net",
                            SocketV2Url = "wss://socket.btcmarkets.net/v2",
                            //ApiKey = "ac400ad8-6051-4dc9-aaf3-2dd3f8a4c0d6",
                            //Secret = "zE4rPkfizqOYQvbYQhOths6KiS2SyBKI3zRbdbu5qM1ha4VgPu4Om/9zaUAuFm80zGCiVSbSD0NK/ar3BWzpJg=="
                        };

                        _client = new BtcMarketsClient(apiSettings);
                    }
                    return _client;
                }
            }
        }
    }
}
