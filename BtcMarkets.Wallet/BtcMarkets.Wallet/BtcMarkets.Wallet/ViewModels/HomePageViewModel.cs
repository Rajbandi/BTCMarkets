using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public bool IsAccountValid
        {
            get
            {
                var isValid = false;
                var data = AppData.Current;
                
                if (!data.IsAccountSetup)
                {
                    AccountErrorMessage = "You need to setup  api keys in settings to retrieve your Account information (balances, open orders, history etc).";
                }

                isValid = data.IsAccountSetup;

                return isValid;
            }
        }

        
        public string AccountErrorMessage { get; private set; }
        public HomePageViewModel() : base("BTC Markets")
        {

            var appData = AppData.Current;

            BtcAudCoin = _btcMarket;

            if (appData.Markets == null || !appData.Markets.Any())
            {
                RefreshMarkets();
            }

           
        }


        private Market _btcMarket => AppData.Current.Markets.FirstOrDefault(x => x.Instrument == Constants.Btc && x.Currency == Constants.Aud) ?? new Market();

        private Market _btcAudCoin;
        public Market BtcAudCoin
        {
            get
            {
                return _btcAudCoin;
            }
            private set
            {
                _btcAudCoin = value;
                OnPropertyChanged("BtcAudCoin");
            }

        }
        public ObservableCollection<MarketNewsItem> MarketNews { get; private set; }


        
        private void LoadMarketNews()
        {
            var appData = AppData.Current;
            var marketNews = appData?.Settings?.Config?.MarketNews;
            if (marketNews != null)
            {
                MarketNews = new ObservableCollection<MarketNewsItem>(marketNews);
            }
            else
            {
                MarketNews = new ObservableCollection<MarketNewsItem>();
            }

            OnPropertyChanged("MarketNews");
        }

        public  void RefreshMarkets(bool isPull = false)
        {
           

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!isPull)
                    IsBusy = true;
                else
                    IsRefreshing = true;

                await Task.Delay(100);
                var appData = AppData.Current;
                await appData.RefreshMarkets();
                BtcAudCoin = _btcMarket;
                LoadMarketNews();

                if (!isPull)
                    IsBusy = false;
                else
                    IsRefreshing = false;

            });
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

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(() =>
               {
                   RefreshMarkets(true);
               });
            }
        }
    }
}
