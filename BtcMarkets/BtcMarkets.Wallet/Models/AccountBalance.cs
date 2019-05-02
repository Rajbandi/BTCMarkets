using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    [DataContract]
    public class AccountBalance
    {
        [DataMember(Name = "balance")]
        public long Balance { get; set; }

        [DataMember(Name = "balancedecimal")]
        public double BalanceDecimal  { get; set; }

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

        [DataMember(Name = "pendingFunds")]
        public long PendingFunds { get; set; } 

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "currencysymbol")]
        public string CurrencySymbol { get; set; }

        [DataMember(Name = "image")]
        public string Image { get; set; }

        [DataMember(Name = "totalaud")]
        public double TotalAud { get; set; }

        [DataMember(Name = "totalaudstring")]
        public string TotalAudString => $"{TotalAud:0.00}";

        [DataMember(Name = "totalbtc")]
        public double TotalBtc { get; set; }

        [DataMember(Name = "totalbtcstring")]
        public string TotalBtcString => $"{TotalBtc:0.000000000}";
    }
}
