using BtcMarkets.Wallet.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.ViewModels
{
    public class BaseMarketViewModel : BaseViewModel
    {
        public BaseMarketViewModel() : this("Markets")
        {


        }

        public BaseMarketViewModel(string title) : base(title)
        {
            IsSearchBarVisible = false;
            AppData.Current.MarketsUpdated += Current_MarketsUpdated;
         
            
        }

        private void Current_MarketsUpdated(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Markets));
        }

        public ObservableCollection<Market> Markets => GetMarkets();
        public virtual ObservableCollection<Market> GetMarkets()
        {
            return new ObservableCollection<Market>();
        }

        private Market _selectedMarket;
        public Market SelectedMarket
        {
            get => _selectedMarket;
            set => SetProperty(ref _selectedMarket, value, nameof(SelectedMarket));
        }

        private bool _isSearchBarVisible;
        public bool IsSearchBarVisible
        {
            get => _isSearchBarVisible;
            set => SetProperty(ref _isSearchBarVisible, value);
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
                    if (market != null)
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
                    if (market != null)
                    {
                        market.Notification = !market.Notification;
                    }

                });
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing; 
            set
            {
                SetProperty(ref _isRefreshing, value, nameof(IsRefreshing));
            }
        }

      
        public void Refresh(bool pullToRefresh = false)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                
                if (!pullToRefresh)
                {
                    IsBusy = true;
                }
                else
                    IsRefreshing = true;

                await RefreshMarkets();

                if (!pullToRefresh)
                    IsBusy = false;
                else
                 IsRefreshing = false;

              
            });
        }
        public ICommand RefreshDataCommand
        {
            get
            {

                return new Command(() =>
                {
                    Refresh(true);

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
