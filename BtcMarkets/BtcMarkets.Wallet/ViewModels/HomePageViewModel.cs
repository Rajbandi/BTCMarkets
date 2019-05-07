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

              //  UpdateHoldings();

                //   if (!isPull)
                IsBusy = false;
                //  else
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
