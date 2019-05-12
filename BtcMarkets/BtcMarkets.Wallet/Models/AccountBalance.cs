using BtcMarkets.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Models
{
    [DataContract]
    public class AccountBalance : BaseBindableObject
    {
        private long _balance;

        [DataMember(Name = "balance")]
        public long Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value, nameof(Balance));
        }


        private double _balanceDecimal;

        [DataMember(Name = "balancedecimal")]
        public double BalanceDecimal
        {
            get => _balanceDecimal;
            set
            {
                SetProperty(ref _balanceDecimal, value, nameof(BalanceDecimal));
                OnPropertyChanged(nameof(BalanceString));
            }
        }

        [DataMember(Name = "balancestring")]
        public string BalanceString {

            get
            {
                var balance = "";
                if(Currency == Constants.Aud)
                {
                    balance = $"{BalanceDecimal:0.00}";
                }
                else
                    balance = $"{BalanceDecimal:0.00000000}";

                return balance;
            }
        }

        private long _pendingFunds;

        [DataMember(Name = "pendingFunds")]
        public long PendingFunds
        {
            get => _pendingFunds;
            set => SetProperty(ref _pendingFunds, value, nameof(PendingFunds));
        }

        private string _currency;
        [DataMember(Name = "currency")]
        public string Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value, nameof(Currency));
        }

        private string _name;

        [DataMember(Name = "name")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private string _currencySymbol;

        [DataMember(Name = "currencysymbol")]
        public string CurrencySymbol
        {
            get => _currencySymbol;
            set => SetProperty(ref _currencySymbol, value, nameof(CurrencySymbol));
        }

        private string _image;
        [DataMember(Name = "image")]
        public string Image
        {
            get => _image;
            set
            {
                SetProperty(ref _image, value, nameof(Image));
               
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                return AppHelper.GetMarketImage(Currency);
            }
        }
        private double _totalAud;
        [DataMember(Name = "totalaud")]
        public double TotalAud
        {
            get => _totalAud;
            set
            {
                SetProperty(ref _totalAud, value, nameof(TotalAud));
                OnPropertyChanged(nameof(TotalAudString));
            }
        }

        [DataMember(Name = "totalaudstring")]
        public string TotalAudString => $"{TotalAud:0.00}";

        private double _totalBtc;

        [DataMember(Name = "totalbtc")]
        public double TotalBtc
        {
            get => _totalBtc;
            set
            {
                SetProperty(ref _totalBtc, value, nameof(TotalBtc));
                OnPropertyChanged(nameof(TotalBtcString));
            }
        }

        [DataMember(Name = "totalbtcstring")]
        public string TotalBtcString => $"{TotalBtc:0.000000000}";
    }
}
