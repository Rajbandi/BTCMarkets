using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.ViewModels
{
    public class TradeDataViewModel : BaseViewModel
    {
         public ObservableCollection<MarketTradeHistory> TradeHistory { get; set; }

        public ObservableCollection<MarketTradeOrder> SellOrders { get; set; }
        public ObservableCollection<MarketTradeOrder> BuyOrders { get; set; }

        private Market _market;
        public Market TradeMarket
        {
            get => _market;
            set  {

                SetProperty(ref _market, value);
             
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
        public TradeDataViewModel()
        {
            SellOrders = new ObservableCollection<MarketTradeOrder>();
            BuyOrders = new ObservableCollection<MarketTradeOrder>();
            TradeHistory = new ObservableCollection<MarketTradeHistory>();

        }


        public void RefreshData()
        {
            //Device.BeginInvokeOnMainThread(async () =>
            //{
                 Task.Run( async () =>
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
                    foreach(var trade in history)
                    {
                        trade.Currency = market.Instrument;
                        TradeHistory.Add(trade);
                    }

                    HistoryString = $"(Top {TradeHistory.Count})";
                    IsBusy = false;
                });
           // });
        }

    }
}
