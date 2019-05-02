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

        private string _holdings;
        public string TotalHoldings
        {
            get => _holdings;
            set
            {
                SetProperty(ref _holdings, value);
            }
        }

        private bool _isSearchBarVisible;
        public bool IsSearchBarVisible {
            get => _isSearchBarVisible;
            set => SetProperty(ref _isSearchBarVisible, value);
        }

        public ObservableCollection<Market> Markets { get; protected set; }

        public Market SelectedMarket { get; set; }

        public MarketsViewModel()
        {

            Markets = new ObservableCollection<Market>();
            //  var appData = AppData.Current;

            //appData.MarketsUpdated += AppData_MarketsUpdated;
            //appData.MarketUpdated += AppData_MarketUpdated;



            Task.Run(async () =>
            {
                await UpdateMarkets();
            });

        }

        private void AppData_MarketUpdated(object sender, MarketEventArgs e)
        {
            UpdateMarket(e.Market);
        
        }

        
        private void AppData_MarketsUpdated(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                await UpdateMarkets();
            });
        }

        protected virtual void LoadMarkets()
        {
            OnPropertyChanged(nameof(Markets));
        }
    
        public async Task UpdateMarkets()
        {

            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = true;
                   // await Task.Delay(50);

                    var data = AppData.Current;
                    foreach (var market in data.Markets)
                    {
                        market.Starred = data.Favourites.Any(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
                    }
                    LoadMarkets();
                    DisplayHoldings();
                    IsBusy = false;
                    IsRefreshing = false;
                });
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

   
        protected bool _btcHoldings;
        public void DisplayHoldings()
        {
            var appData = AppData.Current;
            if (_btcHoldings)
            {
                TotalHoldings = $"{Constants.BtcSymbol}{appData.TotalHoldingsInBtc:0.00000000}";
            }
            else
            {
                TotalHoldings = $"{Constants.AudSymbol}{appData.TotalHoldingsInAud:0.00}";
            }
        }

        public virtual void SearchMarkets(string coin)
        {

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

        public void Refresh(bool pullToRefresh = false)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if(!pullToRefresh)
                    IsBusy = true;
               
                await RefreshMarkets();
                await UpdateMarkets();
                if (!pullToRefresh)
                    IsBusy = false;
             
            });

        }
        public ICommand RefreshCommand
        {
            get
            {
               
                return new Command( () =>
                {
                    Refresh(true);
                   
                });
            }
        }

        public ICommand RefreshDataCommand
        {
            get
            {

                return new Command(() =>
                {
                    Refresh();

                });
            }
        }

        public ICommand ChangeHoldingsCommand
        {
            get
            {
                return new Command(() =>
                {
                    _btcHoldings = !_btcHoldings;
                    DisplayHoldings();
                });
            }
        }

        public ICommand SearchCommand
        {
            get
            {

                return new Command((arg) =>
                {
                    var a = arg;

                });
            }
        }

        public ICommand ShowSearchCommand
        {
            get
            {
                return new Command((arg) =>
                {
                    IsSearchBarVisible = !IsSearchBarVisible;
                });
            }
        }
    }

}
