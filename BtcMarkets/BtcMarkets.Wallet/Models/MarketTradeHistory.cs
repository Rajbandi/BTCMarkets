using BtcMarkets.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class MarketTradeHistory : BaseBindableObject
    {
        [DataMember(Name = "id")]
        public long? Id { get; set; }

        [DataMember(Name = "creationTime")]
        public long? CreationTime { get; set; }

        [DataMember(Name = "createdDate")]
        public string CreatedDate { get; set; }


        [DataMember(Name = "price")]
        public double? Price { get; set; }

        [DataMember(Name = "pricestring")]
        public string PriceString => AppHelper.FormatNumber(Price.HasValue?Price.Value:0);

        [DataMember(Name = "volume")]
        public double? Volume { get; set; }


        [DataMember(Name = "volumestring")]
        public string VolumeString => AppHelper.FormatNumber(Volume.HasValue?Volume.Value:0, Currency);

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

    }
}
