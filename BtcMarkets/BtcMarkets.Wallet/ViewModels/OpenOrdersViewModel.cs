using BtcMarkets.Core.Helpers;
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
    public class OpenOrdersViewModel : BaseViewModel
    {
        public OpenOrdersViewModel()
        {
            Title = "Open Orders";
            OpenOrders = new ObservableCollection<MarketOrderData>();
            GroupedOrders = new ObservableCollection<Grouping<string, MarketOrderData>>();
            RefreshData();
        }
        public ObservableCollection<MarketOrderData> OpenOrders { get; private set; }

        private ObservableCollection<Grouping<string, MarketOrderData>> _groupedOrders;

        public ObservableCollection<Grouping<string, MarketOrderData>> GroupedOrders
        {
            get => _groupedOrders;
            set => SetProperty(ref _groupedOrders, value, nameof(GroupedOrders));
        }

        public void RefreshData()
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
                       OpenOrders.Clear();

                       if (!isLoaderVisible)
                           AppService.Instance.SetLoaderMessage("Loading Open Orders");

                       var appData = AppData.Current;
                       var orders = await appData.GetOpenOrders();

                       foreach (var order in orders)
                       {
                           OpenOrders.Add(order);
                       }

                       var newOrders = new List<MarketOrderData>
                       {
                           new MarketOrderData
                           {
                               Currency = "AUD",
                               Instrument = "LTC",
                               Id = 12344551,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 3000,
                               Volume = 12,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                            new MarketOrderData
                           {
                               Currency = "AUD",
                               Instrument = "ETH",
                               Id = 12344151,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 2000,
                               Volume = 212,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                             new MarketOrderData
                           {
                               Currency = "BTC",
                               Instrument = "LTC",
                               Id = 12344551,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 1000,
                               Volume = 33,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                               new MarketOrderData
                           {
                               Currency = "AUD",
                               Instrument = "LTC",
                               Id = 12344551,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 3000,
                               Volume = 12,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                            new MarketOrderData
                           {
                               Currency = "AUD",
                               Instrument = "ETH",
                               Id = 12344151,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 2000,
                               Volume = 212,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                             new MarketOrderData
                           {
                               Currency = "BTC",
                               Instrument = "LTC",
                               Id = 12344551,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 1000,
                               Volume = 33,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                               new MarketOrderData
                           {
                               Currency = "AUD",
                               Instrument = "LTC",
                               Id = 12344551,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 3000,
                               Volume = 12,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                            new MarketOrderData
                           {
                               Currency = "AUD",
                               Instrument = "ETH",
                               Id = 12344151,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 2000,
                               Volume = 212,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                             new MarketOrderData
                           {
                               Currency = "BTC",
                               Instrument = "LTC",
                               Id = 12344551,
                               Timestamp = ApiHelper.GetTimeStampLong(),
                               Price = 1000,
                               Volume = 33,
                               Status = "Placed",
                               OrderType = "Limit",
                               OrderSide = "Ask"
                           },
                       };

                       foreach(var order in newOrders)
                       {
                           OpenOrders.Add(order);
                       }

                       var groups  = from order in OpenOrders
                                    group order by order.MarketString into g
                                    select new Grouping<string, MarketOrderData>(g.Key, g);

                       //GroupedOrders.Clear();

                       //foreach(var group in groups)
                       //{
                       //    GroupedOrders.Add(group);
                       //}

                       GroupedOrders = new ObservableCollection<Grouping<string, MarketOrderData>>(groups);
                     

                   }
                   catch (Exception ex)
                   {
                       AppHelper.TrackError(ex);
                       AppService.Instance.ShowError("Something went wrong.");
                   }

                   if (!isLoaderVisible)
                       IsBusy = false;

               });
            });
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

    }
}
