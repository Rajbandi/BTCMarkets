using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Models
{
    public class AccountValue : BaseBindableObject
    {

        public AccountValue()
        {
            Balances = new ObservableCollection<AccountBalance>();
            HoldingMode = Constants.Aud;
           
        }

        private Market _btcAudMarket;
        public Market BtcAudMarket
        {
            get => _btcAudMarket;
            set => SetProperty(ref _btcAudMarket, value, nameof(BtcAudMarket));
        }

        
        private double _accountValueInAud;
        public double AccountValueInAud
        {
            get => _accountValueInAud;
            set
            {
                SetProperty(ref _accountValueInAud, value, nameof(AccountValueInAud));
                OnPropertyChanged(nameof(AccountValueInAudString));
                UpdateHoldings();
            }
        }


        private double _accountValueInBtc;
        public double AccountValueInBtc
        {
            get => _accountValueInBtc;
            set
            {

                SetProperty(ref _accountValueInBtc, value, nameof(AccountValueInBtc));
                OnPropertyChanged(nameof(AccountValueInBtcString));
                UpdateHoldings();
            }
        }

        public string AccountValueInBtcString =>  $"BTC {Constants.BtcSymbol}{AccountValueInBtc:0.00000000}";

        public string AccountValueInAudString =>  $"AUD {Constants.AudSymbol}{AccountValueInAud:0.00}";
        //private string _accountValueInBtcString;
        //public string AccountValueInBtcString
        //{
        //    get => _accountValueInBtcString;
        //    set => SetProperty(ref _accountValueInBtcString, value, nameof(AccountValueInBtcString));
        //}

        //private string _accountValueInAudString;
        //public string AccountValueInAudString
        //{
        //    get => _accountValueInAudString;
        //    set => SetProperty(ref _accountValueInAudString, value, nameof(AccountValueInAudString));
        //}

        private ObservableCollection<AccountBalance> _balances;

        public ObservableCollection<AccountBalance> Balances
        {
            get => _balances;
            set => SetProperty(ref _balances, value, nameof(Balances));
        }

        private string _holdingsMode;
        public string HoldingMode
        {
            get => _holdingsMode;
            set => SetProperty(ref _holdingsMode, value, nameof(HoldingMode));
        }

        private string _holdingsValue;
        public string HoldingsValue
        {
            get => _holdingsValue;
            set => SetProperty(ref _holdingsValue, value, nameof(HoldingsValue));
        }

        public void UpdateHoldings()
        {
            if (HoldingMode == Constants.Aud)
            {
          
                HoldingsValue = AccountValueInAudString;
            }
            else
            {
            
                HoldingsValue = AccountValueInBtcString;
            }
        }

        public ICommand ChangeHoldingsCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (HoldingMode == Constants.Btc)
                    {
                      
                       HoldingMode = Constants.Aud;
                      
                    }
                    else
                    {
                        HoldingMode = Constants.Btc;
                    }

                    UpdateHoldings();
                });
            }
        }
    }

}
