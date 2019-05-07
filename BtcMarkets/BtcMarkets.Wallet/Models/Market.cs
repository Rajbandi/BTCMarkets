using System.Runtime.Serialization;
using BtcMarkets.Core.Api.Contracts;
using BtcMarkets.Wallet.Helpers;

namespace BtcMarkets.Wallet.Models
{

    [DataContract(Name = "market")]
    public class Market : BaseBindableObject
    {

        public Market()
        {
            Starred = false;
            Notification = false;
        }

        public string Pair => $"{Instrument}/{Currency}";

        public string Id => $"{Instrument}-{Currency}";

        private string _name;
        [DataMember(Name = "name")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, nameof(Name));
        }

        private string _instrument;
        [DataMember(Name = "instrument")]
        public string Instrument
        {
            get => _instrument;
            set => SetProperty(ref _instrument, value, nameof(Instrument));

        }

        private string _currency = string.Empty;
        [DataMember(Name = "currency")]
        public string Currency
        {
            get => _currency;
            set => SetProperty(ref _currency, value, nameof(Currency));
        }

        private string _image = string.Empty;
        [DataMember(Name = "image")]
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value, nameof(Image));
        }

        private double _bid;

        [DataMember(Name = "bid")]
        public double Bid
        {
            get => _bid;
            set => SetProperty(ref _bid, value, nameof(Bid));
        }

        public string BidString => AppHelper.FormatNumber(Bid, Currency);

        private double _ask;
        [DataMember(Name = "ask")]
        public double Ask
        {
            get => _ask;
            set => SetProperty(ref _ask, value, nameof(Ask));
        }

        public string AskString => AppHelper.FormatNumber(Ask, Currency);

        private double _lastPrice;

        [DataMember(Name = "lastprice")]
        public double LastPrice
        {
            get => _lastPrice;
            set
            {
                SetProperty(ref _lastPrice, value, nameof(LastPrice));
                OnPropertyChanged(nameof(LastPriceString));
                OnPropertyChanged(nameof(LastPriceWithSymbol));
            }
        }
        public string LastPriceString
        {
            get
            {
                var str = AppHelper.FormatNumber(LastPrice, Currency);
                return str;
            }
        }
        public string LastPriceWithSymbol
        {
            get
            {
                var str = $"{CurrencySymbol}{LastPrice:0.00}";
                if (Currency == Constants.Btc)
                {
                    str = $"{CurrencySymbol}{LastPrice:F8}";
                }

                return str;
            }
        }

        private double _volume;
        [DataMember(Name = "volume")]
        public double Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value, nameof(Volume));
        }

        public string VolumeString => AppHelper.FormatNumber(Volume, Instrument);

        private double _holdings;
        public double Holdings
        {
            get => _holdings;
            set => SetProperty(ref _holdings, value, nameof(Holdings));
        }

        public string HoldingsString
        {
            get
            {
                if (Instrument == Constants.Aud)
                {
                    return $"{Holdings:0.00}";
                }
                else
                    return $"{Holdings:0.00000000}";

            }
        }

        private bool _starred;

        [DataMember(Name = "starred")]
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

        [DataMember(Name = "starredicon")]
        public string StarredIcon
        {
            get => _starredIcon;
            set => SetProperty(ref _starredIcon, value, nameof(StarredIcon));
        }

        private ToggleImage _toggleStarred;

        [DataMember(Name = "togglestarred")]
        public ToggleImage ToggleStarred
        {
            get => _toggleStarred;
            set => SetProperty(ref _toggleStarred, value, nameof(ToggleStarred));
        }

        private bool _notification;

        [DataMember(Name = "notification")]
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

        [DataMember(Name = "notificationIcon")]
        public string NotificationIcon
        {
            get => _notificationIcon;
            set => SetProperty(ref _notificationIcon, value, nameof(NotificationIcon));
        }


        private ToggleImage _toggleNotification;

        public ToggleImage ToggleNotification
        {
            get => _toggleNotification;
            set => SetProperty(ref _toggleNotification, value, nameof(ToggleNotification));
        }

        [DataMember(Name = "PreviousPrice")]
        private double _previousPrice;
        public double PreviousPrice
        {
            get => _previousPrice;
            set => SetProperty(ref _previousPrice, value, nameof(PreviousPrice));
        }

        [DataMember(Name = "Change")]
        private double _change;
        public double Change
        {
            get => _change;
            set => SetProperty(ref _change, value, nameof(Change));
        }

        public string ChangeString => AppHelper.DoubleToPercentageString(Change);

        [DataMember(Name = "InstrumentSymbol")]
        public string InstrumentSymbol
        {
            get
            {

                var currency = "  ";

                if (Instrument == Constants.Btc)
                {
                    currency = Constants.BtcSymbol;
                }

                return currency;
            }
        }


        [DataMember(Name = "CurrencySymbol")]
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

        [DataMember(Name = "FullName")]
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
