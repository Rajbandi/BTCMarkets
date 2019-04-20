using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class MarketOrder : BaseBindableObject
    {
        private double _rate;
        [JsonProperty("rate")]
        public double Rate
        {
            get => _rate;
            set => SetProperty(ref _rate, value);

        }

        private double _amount;
        [JsonProperty("amount")]
        public double Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);

        }
    
    }
}
