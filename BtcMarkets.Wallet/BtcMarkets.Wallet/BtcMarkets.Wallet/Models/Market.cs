using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BtcMarkets.Core.Api.Contracts;
using BtcMarkets.Wallet.Helpers;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Models
{
    
    [JsonObject("market")]
    public class Market : BaseBindableObject
    {

        public Market()
        {
            Starred = false;
            Notification = false;
        }

        public string Pair => $"{Instrument}/{Currency}";

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _instrument;
        [JsonProperty("instrument")]
        public string Instrument
        {
            get => _instrument;
            set => SetProperty(ref _instrument, value);

        }

        private string _currency = string.Empty;
        [JsonProperty("currency")]
        public string Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value);
        }

        private string _image = string.Empty;
        [JsonProperty("image")]
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

       

        private double _bid;

        [JsonProperty("bid")]
        public double Bid
        {
            get => _bid;
            set => SetProperty(ref _bid, value);
        }

        public string BidString => AppHelper.FormatNumber(Bid, Currency);

        private double _ask;
        [JsonProperty("ask")]
        public double Ask
        {
            get => _ask;
            set => SetProperty(ref _ask, value);
        }

        public string AskString => AppHelper.FormatNumber(Ask, Currency);

        private double _lastPrice;

        [JsonProperty("lastprice")]
        public double LastPrice
        {
            get => _lastPrice;
            set => SetProperty(ref _lastPrice, value);
        }
        public string LastPriceString
        {
            get
            {
                var str = AppHelper.FormatNumber(LastPrice, Currency);
                return str;
            }
        }
        public string LastPriceWithSymbol {
            get
            {
                var str = $"{CurrencySymbol}{LastPrice}";
                if(Currency == Constants.Btc)
                {
                    str = $"{CurrencySymbol}{LastPrice:F8}";
                }
                return str;
            }
        }


        private double _volume;

        [JsonProperty("volume")]
        public double Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value);
        }

        public string VolumeString => AppHelper.FormatNumber(Volume, Instrument);

        private double _holdings;

        public double Holdings
        {
            get => _holdings;
            set => SetProperty(ref _holdings, value);
        }

        public string HoldingsString => $"{Holdings:F2}";

        private bool _starred;

        [JsonProperty("starred")]
        public bool Starred
        {
            get => _starred;
            set
            {
                SetProperty(ref _starred, value);
                StarredIcon = value ? "favorite" : "favorite_border";
               
                ToggleStarred = new ToggleImage
                {
                    FontFamily = "MaterialDesign",
                    Value = value,
                    OnImage = "favorite",
                    OffImage = "favorite_border",
                    Color = "DefaultTextColor"
                };
            }
        }

        private string _starredIcon;

        [JsonProperty("starredicon")]
        public string StarredIcon
        {
            get => _starredIcon;
            set => SetProperty(ref _starredIcon, value);
        }

        private ToggleImage _toggleStarred;

        [JsonProperty("togglestarred")]
        public ToggleImage ToggleStarred
        {
            get => _toggleStarred;
            set => SetProperty(ref _toggleStarred, value);
        }

        private bool _notification;

        [JsonProperty("notification")]
        public bool Notification
        {
            get => _notification;
            set
            {
                SetProperty(ref _notification, value);
                NotificationIcon = value ? "notifications_active" : "notifications_none";
                ToggleNotification = new ToggleImage
                {
                    FontFamily = "MaterialDesign",
                    Value = value,
                    OnImage = "notifications_active",
                    OffImage = "notifications_none",
                    Color = "AccentColor"
                };
            }
        }

        private string _notificationIcon;

        [JsonProperty("notificationIcon")]
        public string NotificationIcon
        {
            get => _notificationIcon;
            set => SetProperty(ref _notificationIcon, value);
        }


        private ToggleImage _toggleNotification;

        public ToggleImage ToggleNotification
        {
            get => _toggleNotification;
            set => SetProperty(ref _toggleNotification, value);
        }

       

   

        [JsonProperty("CurrencySymbol")]
        public string CurrencySymbol
        {
            get
            {
                var currency = (Currency ?? "").ToUpper();
                var currencyString = currency;
                if (currency == Constants.Aud)
                {
                    currencyString = $"$";
                }
                else if (currency == Constants.Btc)
                {
                    currencyString = $"Ƀ";
                }

                return currencyString;
            }
        }

        [JsonProperty("FullName")]
        public string FullName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name))
                    return $"{Name}({Instrument})";
                else
                    return $"{Instrument}";
            }
        }
      
        public static Market Load(MarketTick tick)
        {
            Market market = null;

            if (tick != null)
            {
                market = new Market();
                market.Instrument = tick.Instrument;
                market.Currency = tick.Currency;
                market.LastPrice = tick.LastPrice;
                market.Bid = tick.BestBid;
                market.Ask = tick.BestAsk;
                market.Volume = tick.Volume24h;
                
            }

            return market;
        }
        
    }
}
