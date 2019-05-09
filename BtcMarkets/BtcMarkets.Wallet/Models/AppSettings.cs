using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BtcMarkets.Wallet.Models
{
    [DataContract]
    public class ApiCredentials
    {
        [DataMember(Name = "apikey")]
        public string ApiKey { get; set; }

        [DataMember(Name = "secret")]
        public string Secret { get; set; }

        public ApiCredentials()
        {
            ApiKey = "";
            Secret = "";
        }
    }

    [JsonObject]
    [Serializable]
    [DataContract]
    public class AppSettings
    {
        [DataMember(Name = "reset")]
        public bool Reset { get; set; }

        [DataMember(Name = "socketon")]
        public bool LiveUpdates { get; set; } = true; 

        [DataMember(Name = "favourites")]
        public List<MarketFavourite> Favourites { get; private set; }

        [JsonIgnore]
        public ApiCredentials ApiCredentials { get; set; }
      
        [DataMember(Name = "config")]
        public CoinConfig Config{ get; set; }

        [DataMember(Name = "theme")]
        public string Theme { get; set; }
        public AppSettings()
        {
            Favourites = new List<MarketFavourite>();
            LiveUpdates = true;
            Theme = "Dark";
            ApiCredentials = new ApiCredentials();
        }
        
        public void AddFavourite(MarketFavourite market)
        {
            var favourite = Favourites.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);

            if(favourite == null)
            {
                Favourites.Add(market);
            }
        }
        public void RemoveFavourite(MarketFavourite market)
        {
            var favourite = Favourites.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);

            if (favourite != null)
            {
                Favourites.Remove(favourite);
            }
        }

    }
}
