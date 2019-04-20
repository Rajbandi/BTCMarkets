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
        public string PriceString => AppHelper.FormatNumber(Price);


        [DataMember(Name = "volume")]
        public double Volume { get; set; }

        [DataMember(Name = "volumestring")]
        public string VolumeString => AppHelper.FormatNumber(Volume, Currency);


        [DataMember(Name = "value")]
        public double Value { get; set; }

        [DataMember(Name = "valuestring")]
        public string ValueString => AppHelper.FormatNumber(Value);


        [DataMember(Name = "currency")]
        public string Currency { get; set; }
    }
}
