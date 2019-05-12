using BtcMarkets.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    [DataContract]
    public class MarketTradeOrder : BaseBindableObject
    {
        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "pricestring")]
        public string PriceString => AppHelper.FormatNumber(Price, Currency);


        [DataMember(Name = "volume")]
        public double Volume { get; set; }

        [DataMember(Name = "volumestring")]
        public string VolumeString => AppHelper.FormatNumber(Volume, Instrument);


        [DataMember(Name = "value")]
        public double Value { get; set; }

        [DataMember(Name = "valuestring")]
        public string ValueString => AppHelper.FormatNumber(Value,Currency);

        [DataMember(Name = "total")]
        public double Total { get; set; }

        [DataMember(Name = "totalstring")]
        public string TotalString => AppHelper.FormatNumber(Price * Volume, Currency);

        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }


        [DataMember(Name = "currencysymbol")]
        public string CurrencySymbol => Currency == Constants.Btc ? Constants.BtcSymbol : Constants.AudSymbol;

    }
}
