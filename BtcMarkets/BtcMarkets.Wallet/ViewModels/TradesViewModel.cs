using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.ViewModels
{
    public class TradesViewModel : BaseViewModel
    {
        public List<Market> TradeMarkets => AppData.Current.Markets;


        private bool _isMarketsVisible;

        public bool IsMarketsVisible
        {
            get =>  _isMarketsVisible;
            set => SetProperty(ref _isMarketsVisible, value, nameof(IsMarketsVisible));
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
                if (_tradingMarketPair != value)
                {
                    SetProperty(ref _tradingMarketPair, value, nameof(TradingMarketPair));
                }
            }
        }
        public ObservableCollection<MarketTradePair> TradeMarketPairs { get; private set; }

        public TradesViewModel(Market market = null)
        {
            Title = "Trades";
            
            TradeMarketPairs = new ObservableCollection<MarketTradePair>();
            SellOrders = new ObservableCollection<MarketTradeOrder>();
            BuyOrders = new ObservableCollection<MarketTradeOrder>();
            TradeHistory = new ObservableCollection<MarketTradeHistory>();

            MarketTradePair marketPair = null;
            if(market != null)
            {
                marketPair = new MarketTradePair
                {
                    Pair = market.Pair,
                    Image = market.Image,
                };
            }
            IsMarketsVisible = marketPair == null;
            RefreshMarkets(marketPair);
        }

        public ObservableCollection<MarketTradeHistory> TradeHistory { get; set; }

        public ObservableCollection<MarketTradeOrder> SellOrders { get; set; }
        public ObservableCollection<MarketTradeOrder> BuyOrders { get; set; }

        private Market _market;
        public Market TradeMarket
        {
            get => _market;
            set
            {
                SetProperty(ref _market, value, nameof(TradeMarket));
            }
        }

        private string _sellString;
        public string SellString
        {
            get => _sellString;
            set => SetProperty(ref _sellString, value);
        }
        private string _buyString;
        public string BuyString
        {
            get => _buyString;
            set => SetProperty(ref _buyString, value);
        }

        private string _historyString;
        public string HistoryString
        {
            get => _historyString;
            set => SetProperty(ref _historyString, value);
        }
      
        public void RefreshData()
        {
            //Device.BeginInvokeOnMainThread(async () =>
            //{
                 Task.Run(async () =>
                {
                    IsBusy = true;

                    AppService.Instance.SetLoaderMessage("Loading Trades");

                    var appData = AppData.Current;

                    var market = TradeMarket;

                    var newMarket = await appData.GetMarket(market.Instrument, market.Currency);

                    appData.UpdateMarket(newMarket);

                    TradeMarket = appData.Markets.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);



                    var orders = await appData.GetMarketTradeOrders(market);


                    BuyOrders.Clear();
                    foreach (var order in orders.Buy)
                    {
                        order.Currency = market.Instrument;
                        BuyOrders.Add(order);
                    }


                    BuyString = $"(Top {BuyOrders.Count})";

                    SellOrders.Clear();
                    foreach (var order in orders.Sell)
                    {
                        order.Currency = market.Instrument;
                        SellOrders.Add(order);
                    }

                    SellString = $"(Top {SellOrders.Count})";

                    TradeHistory.Clear();

                    var history = await appData.GetMarketTradeHistory(market);
                    foreach (var trade in history)
                    {
                        trade.Currency = market.Instrument;
                        TradeHistory.Add(trade);
                    }

                    HistoryString = $"(Top {TradeHistory.Count})";
                    IsBusy = false;
                });
            //});
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
           
        }

        public void InitMarket()
        {
            if (TradingMarketPair == null)
            {
                var firstPair = TradeMarketPairs?.FirstOrDefault();
                if (firstPair != null)
                {
                    TradingMarketPair = firstPair;
                    //  RefreshMarket(markets.FirstOrDefault());
                }
            }
        }

        public void RefreshMarket(Market market)
        {
            if (market == null)
                return;

            var m = TradeMarkets.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
            if(m != null)
            {
                TradeMarket = m;
                RefreshData();
                
            }
        }
      

        public void RefreshMarketPair(MarketTradePair marketPair)
        {
            if (marketPair == null)
                return;

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

        public ICommand RefreshDataCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        RefreshData();
                      //  OnPropertyChanged(nameof(TradeMarket));
                    });

                });
            }
        }

        public ICommand SelectedCommand
        {
            get
            {
                return new Command((arg) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var pair = arg as MarketTradePair;
                        if(pair != null)
                        {
                            // TradingMarketPair = pair;
                            RefreshMarkets(pair);
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
                        RefreshMarketPair(TradingMarketPair);
                    });

                });
            }
        }
    }
}
