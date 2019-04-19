using BtcMarkets.Wallet.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.ViewModels
{
    public class TradesViewModel : TradeDataViewModel
    {

        private List<Market> _tradeMarkets;
        public List<Market> TradeMarkets => AppData.Current.Markets;
       

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
                    SetProperty(ref _tradingMarketPair, value);
                    if(_tradingMarketPair != null && TradeMarketPairs != null && TradeMarketPairs.Any())
                    {

                        var accentStyle = Application.Current.Resources["DefaultAccentText"] as Style; 
                        var defaultStyle = Application.Current.Resources["SmallDefaultText"] as Style;
                        foreach (var pair in TradeMarketPairs)
                        {

                            if (pair.Pair == _tradingMarketPair.Pair)
                                pair.Style = accentStyle;
                            else
                                pair.Style = defaultStyle;
                        }
                    }
                    RefreshMarketPair(value);
                }
            }
        }
        public ObservableCollection<MarketTradePair> TradeMarketPairs { get; private set; }

        public TradesViewModel()
        {
            TradeMarketPairs = new ObservableCollection<MarketTradePair>();

            RefreshMarkets();
        }
               

        public void RefreshMarkets()
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
          
            var firstPair = TradeMarketPairs.FirstOrDefault();
            if (firstPair != null)
            {
                TradingMarketPair = firstPair;
              //  RefreshMarket(markets.FirstOrDefault());
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

        public ICommand RefreshCommand
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
                            TradingMarketPair = pair;
                        }
                    });

                });
            }
        }
    }
}
