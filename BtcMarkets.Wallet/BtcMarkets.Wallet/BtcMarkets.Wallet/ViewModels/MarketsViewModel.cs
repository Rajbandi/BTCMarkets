using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BtcMarkets.Wallet.ViewModels
{
    public class MarketsViewModel : BaseViewModel
    {
       
        public ObservableCollection<Market> Markets { get; protected set; }

        public Market SelectedMarket { get; set; }

        public MarketsViewModel()
        {

            Markets = new ObservableCollection<Market>();
          //  var appData = AppData.Current;

            //appData.MarketsUpdated += AppData_MarketsUpdated;
            //appData.MarketUpdated += AppData_MarketUpdated;
          

            UpdateMarkets();
           
        }

        private void AppData_MarketUpdated(object sender, MarketEventArgs e)
        {
            UpdateMarket(e.Market);
        
        }

        private void AppData_MarketsUpdated(object sender, EventArgs e)
        {
            UpdateMarkets();
          
        }

        protected virtual void LoadMarkets()
        {

        }
        public void UpdateMarkets()
        {
           
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                await Task.Delay(50);

                var data = AppData.Current;
                foreach (var market in data.Markets)
                {
                    market.Starred = data.Favourites.Any(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
                }
                LoadMarkets();
                OnPropertyChanged("Markets");
                IsBusy = false;

            });
        }
        public void UpdateMarket(Market market)
        {
            if (market == null)
                return;

            var m = Markets.IndexOf(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
            if (m < 0)
            {
                Markets.Add(market);
            }
            else
            {
                Markets[m] = market;
            }

        }

        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public async Task RefreshMarkets()
        {
            var appData = AppData.Current;

          
            await appData.RefreshMarkets();
            
           

        }


        public ICommand MarketDetailsCommand
        {
            get
            {
                return new Command((item) =>
                {
                    var m = (Market)item;
                    var market = SelectedMarket;
                });
            }
        }

        public ICommand FavouriteCommand
        {
            get
            {
                return new Command((item) =>
                {
                    var market = (Market)item;
                    if(market != null)
                    {
                       var state = market.Starred;


                        market.Starred = !state;

                        var data = AppData.Current;

                        data.AddOrRemoveFavourite(market, state);

                    }
                });
            }
        }
        public ICommand NotificationCommand
        {
            get
            {
                return new Command((item) =>
                {
                    var market = (Market)item;
                    if(market != null)
                    {
                        market.Notification = !market.Notification;
                    }

                });
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
               
                return new Command( () =>
                {

                    Device.BeginInvokeOnMainThread(async () =>
                   {
                       IsRefreshing = true;
                       await RefreshMarkets();
                       IsRefreshing = false;
                   });

                });
            }
        }
    }

}
