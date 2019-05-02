using BtcMarkets.Core.Helpers;
using BtcMarkets.Wallet.Helpers;
using System.Runtime.Serialization;

namespace BtcMarkets.Wallet.Models
{
    [DataContract]
    public class MarketOrderData : BaseBindableObject
    {
      
        [DataMember(Name ="id")]
        public long Id { get; set; }
        
        
        [DataMember(Name = "instrument")]
        public string Instrument
        {
            get ;
            set ;
        }

        
        [DataMember(Name = "currency")]
        public string Currency
        {
            get ;
            set ;
        }


        
        [DataMember(Name = "ordertype")]
        public string OrderType
        {
            get ;
            set ;
        }

        

        [DataMember(Name = "orderside")]
        public string OrderSide
        {
            get ;
            set ;
        }

        

        [DataMember(Name = "timestamp")]
        public long Timestamp
        {
            get ;
            set ;
        }

        

        [DataMember(Name = "status")]
        public string Status
        {
            get;
            set;
        }

        

        [DataMember(Name = "volume")]
        public double Volume
        {
            get ;
            set;
        }

        

        [DataMember(Name = "openvolume")]
        public double OpenVolume
        {
            get;
            set;
        }

        

        [DataMember(Name = "price")]
        public double Price
        {
            get ;
            set ;
        }

        public string PriceString => AppHelper.FormatNumber(Price);

        public string VolumeString => AppHelper.FormatNumber(Volume, Instrument);

        public string MarketString
        {
            get
            {
                var market = string.Empty;
                if(!string.IsNullOrWhiteSpace(Currency) && !string.IsNullOrWhiteSpace(Instrument))
                {
                    market = $"{Instrument}-{Currency}";
                }

                return market;
            }
        }

        public string OrderSideString
        {
            get
            {
                var value = string.Empty;
                if (!string.IsNullOrWhiteSpace(OrderType) && !string.IsNullOrWhiteSpace(OrderSide))
                {
                    value = $"{OrderType} {OrderSide}";
                }
                return value;
            }
        }

        public string DateString
        {
            get
            {
                var value = ApiHelper.ToLocalTime(Timestamp).ToString("dd-MMM-yyyy HH:mm");
                return value;
            }
        }
    }
}
