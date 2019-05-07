using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace BtcMarkets.Wallet.ViewModels
{
    public class OrdersHistoryViewModel : BaseViewModel
    {
        public OrdersHistoryViewModel()
        {
            Title = "Orders History";
            TradeMarketPairs = new ObservableCollection<MarketTradePair>();
            OrderHistory = new ObservableCollection<MarketOrderData>();
            RefreshMarkets();
        }

        private MarketTradePair _tradingMarketPair;
        public MarketTradePair TradingMarketPair
        {
            get
            {
                return _tradingMarketPair;
            }
            set
            {
                SetProperty(ref _tradingMarketPair, value, nameof(TradingMarketPair));
            }
        }

        public ObservableCollection<MarketOrderData> OrderHistory { get; private set; }

        private ObservableCollection<MarketTradePair> _tradeMarketPairs;
        public ObservableCollection<MarketTradePair> TradeMarketPairs
        {
            get => _tradeMarketPairs;
            set => SetProperty(ref _tradeMarketPairs, value, nameof(TradeMarketPairs));
        }

        
        public ObservableCollection<Grouping<string, MarketOrderData>> GroupedOrders
        {
            get => _groupedOrders;
            set => SetProperty(ref _groupedOrders, value, nameof(GroupedOrders));
        }

        public List<Market> TradeMarkets => AppData.Current.Markets;

        private Market _market;
        private ObservableCollection<Grouping<string, MarketOrderData>> _groupedOrders;
        private bool _hasMore;

        public Market TradeMarket
        {
            get => _market;
            set
            {

                SetProperty(ref _market, value);

            }
        }

        public bool HasMoreOrders
        {
            get => _hasMore;
            set => SetProperty(ref _hasMore, value, nameof(HasMoreOrders));
        }

        public void RefreshMarkets(MarketTradePair marketPair = null)
        {

            TradeMarketPairs.Clear();

            var markets = TradeMarkets.OrderBy(x => x.Currency);
            foreach (var market in markets)
            {
                var pair = $"{market.Instrument}/{market.Currency}";
                var tradePair = new MarketTradePair
                {
                    Pair = pair,
                    Image = market.Image,
                    Style = Application.Current.Resources["SmallDefaultText"] as Style
                };
                TradeMarketPairs.Add(tradePair);
            }

            if(marketPair != null)
            {
                TradingMarketPair = marketPair;
            }
            else
            {
                InitMarket();
            }
        }

        public void InitMarket()
        {
            if (TradingMarketPair == null)
            {
                var firstPair = TradeMarketPairs?.FirstOrDefault();
                if (firstPair != null)
                {
                    TradingMarketPair = firstPair;
                    RefreshMarketPair();
                }
            }
            
        }
        public void RefreshMarket(Market market)
        {
            if (market == null)
                return;

            var m = TradeMarkets.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
            if (m != null)
            {
                TradeMarket = m;
                RefreshData();

            }
        }


        public void RefreshMarketPair(MarketTradePair marketPair = null)
        {
            try
            {
                if (marketPair == null)
                    marketPair = TradingMarketPair;

                var selectedMarket = marketPair.Pair;

                if (!string.IsNullOrWhiteSpace(selectedMarket))
                {
                    var parts = selectedMarket.Split("/".ToCharArray());
                    if (parts.Length > 1)
                    {
                        RefreshMarket(new Market
                        {
                            Instrument = parts[0],
                            Currency = parts[1]
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError();                
            }
        }

        public void RefreshData(long? since = null)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Run(async () =>
                {
                    var isLoaderVisible = AppService.Instance.IsLoaderShowing;

                    if (!isLoaderVisible)
                        IsBusy = true;


                    try
                    {
                        if (!since.HasValue)
                        {
                            OrderHistory.Clear();
                        }

                        //if (!isLoaderVisible)
                        //   AppHelper.SetLoaderMessage("Loading Orders History");

                        var appData = AppData.Current;

                        var orders = await appData.GetOrderHistory(TradeMarket.Instrument, TradeMarket.Currency, since);


                        foreach (var order in orders)
                        {
                            OrderHistory.Add(order);
                        }

                        HasMoreOrders = OrderHistory.Count >= 200;
                    }
                    catch (Exception ex)
                    {
                        AppHelper.TrackError(ex);
                        AppHelper.ShowError("Something went wrong.");
                    }

                    if (!isLoaderVisible)
                        IsBusy = false;

                });
            });
        }

        public ICommand RefreshDataCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        RefreshData();

                    });

                });
            }
        }
        public ICommand ViewMoreCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var lastTrade = OrderHistory.LastOrDefault();
                        if (lastTrade != null)
                        {
                            var since = lastTrade.Id;
                            RefreshData(since);
                        }
                    });
                });
            }
        }
        public ICommand MarketChangedCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        RefreshMarketPair();
                    });
                });
            }
        }
    }
}
