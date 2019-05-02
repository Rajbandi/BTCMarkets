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
        public HomePageViewModel() : base("BTC Markets")
        {

            var appData = AppData.Current;

            Holdings = new ObservableCollection<AccountBalance>();

            BtcAudCoin = _btcMarket;
            IsRefreshing = false;
            if (appData.Markets == null || !appData.Markets.Any())
            {
                RefreshMarkets();
            }

            if(!appData.IsAccountSetup)
            {
                AccountErrorMessage = "You need to setup  api keys in settings to retrieve your \r\nAccount information (balances, open orders, history etc).";
                _isAccountInValid = true;
            }

        }

        public string NoBalanceMessage => "You have no account balance(s).";

        public string SetupApiKeysText => "Setup Api Keys";

        private bool _isAccountInValid;
        public bool IsAccountInvalid
        {
            get => _isAccountInValid;
            private set => SetProperty(ref _isAccountInValid, value);
        }

        private string _accountErrorMessage;
        public string AccountErrorMessage
        {
            get => _accountErrorMessage;
            private set => SetProperty(ref _accountErrorMessage, value);
        }

        private Market _btcMarket => AppData.Current.Markets.FirstOrDefault(x => x.Instrument == Constants.Btc && x.Currency == Constants.Aud) ?? new Models.Market();

        private Market _btcAudCoin;
        public Market BtcAudCoin
        {
            get => _btcAudCoin;
            private set => SetProperty(ref _btcAudCoin, value);
        }

        public ObservableCollection<AccountBalance> Holdings { get; private set; }
        public ObservableCollection<MarketNewsItem> MarketNews { get; private set; }

        private string _holdings;

        public string TotalHoldings
        {
            get => _holdings;
            set => SetProperty(ref _holdings, value);
        }
        
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


        public void RefreshMarkets(bool isPull = false)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                // if (!isPull)
                IsBusy = true;
                //  else
                //  IsRefreshing = true;

                await Task.Delay(100);
                var appData = AppData.Current;
                await appData.RefreshMarkets();
                BtcAudCoin = _btcMarket;
                LoadMarketNews();

                DisplayHoldings();

                //   if (!isPull)
                IsBusy = false;
                //  else
                IsRefreshing = false;

            });
        }

        private bool _btcHoldings;
        public void DisplayHoldings(bool refresh = true)
        {
            var appData = AppData.Current;
            if (_btcHoldings)
            {
                TotalHoldings = $"BTC {Constants.BtcSymbol}{appData.TotalHoldingsInBtc:0.00000000}";
            }
            else
            {
                TotalHoldings = $"AUD {Constants.AudSymbol}{appData.TotalHoldingsInAud:0.00}";
            }

            if (refresh)
            {
                Holdings.Clear();
                var balances = appData.Balances.Where(x => x.Balance > 0).OrderBy(x => x.Currency);
                var audBalance = balances.FirstOrDefault(x => x.Currency == Constants.Aud);
                if (audBalance != null)
                {
                    Holdings.Add(audBalance);
                }
                var btcBalance = balances.FirstOrDefault(x => x.Currency == Constants.Btc);
                if (btcBalance != null)
                {
                    Holdings.Add(btcBalance);
                }
                foreach (var balance in balances.Where(x => x.Currency != Constants.Aud && x.Currency != Constants.Btc))
                {
                    Holdings.Add(balance);
                }
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


        public ICommand ChangeHoldingsCommand
        {
            get
            {
                return new Command(() =>
                {
                    _btcHoldings = !_btcHoldings;
                    DisplayHoldings(false);
                });
            }
        }
        public ICommand RefreshBalancesCommand
        {
            get
            {
                return new Command(() =>
                {
                    DisplayHoldings();
                });
            }
        }
    }
}
