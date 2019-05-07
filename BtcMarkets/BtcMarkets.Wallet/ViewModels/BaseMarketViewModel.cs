using BtcMarkets.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BtcMarkets.Wallet.ViewModels
{
    public class BaseMarketViewModel : BaseViewModel
    {
        public BaseMarketViewModel()
        {

        }

        public BaseMarketViewModel(string title) : base(title)
        {

        }

        private AccountValue _accountValue;
        public AccountValue AccountHoldings
        {
            get => _accountValue;
            set => SetProperty(ref _accountValue, value, nameof(AccountHoldings));
        }


        public void UpdateHoldings()
        {
            if (AccountHoldings == null)
            {
                AccountHoldings = new AccountValue();
            }

            var appData = AppData.Current;

            AccountHoldings.AccountValueInAud = appData.TotalHoldingsInAud;
            AccountHoldings.AccountValueInBtc = appData.TotalHoldingsInBtc;

            var accountBalances = new List<AccountBalance>();

            var balances = appData.Balances.Where(x => x.Balance > 0).OrderBy(x => x.Currency);
            var audBalance = balances.FirstOrDefault(x => x.Currency == Constants.Aud);
            if (audBalance != null)
            {
                accountBalances.Add(audBalance);
            }
            var btcBalance = balances.FirstOrDefault(x => x.Currency == Constants.Btc);
            if (btcBalance != null)
            {
                accountBalances.Add(btcBalance);
            }
            foreach (var balance in balances.Where(x => x.Currency != Constants.Aud && x.Currency != Constants.Btc))
            {
                accountBalances.Add(balance);
            }
            AccountHoldings.Balances = new ObservableCollection<AccountBalance>(accountBalances);

            OnPropertyChanged(nameof(AccountHoldings));
        }

        
    }
}
