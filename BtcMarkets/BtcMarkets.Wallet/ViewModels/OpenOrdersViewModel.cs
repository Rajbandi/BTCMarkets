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
            if (!AppData.Current.CheckInternet())
                return;

            if (!AppData.Current.IsAccountSetup)
                return;

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

                       //if (!isLoaderVisible)
                       //    AppService.Instance.SetLoaderMessage("Loading Open Orders");

                       var appData = AppData.Current;
                       var orders = await appData.GetOpenOrders();

                       foreach (var order in orders)
                       {
                           OpenOrders.Add(order);
                       }

                       var groups  = from order in OpenOrders
                                    group order by order.MarketString into g
                                    select new Grouping<string, MarketOrderData>(g.Key, g);

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

    }
}
