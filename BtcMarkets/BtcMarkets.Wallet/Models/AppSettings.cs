using BtcMarkets.Wallet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BtcMarkets.Wallet.Models
{
    [JsonObject]
    [Serializable]
    public class AppSettings
    {

       public List<MarketFavourite> Favourites { get; private set; }

        public CoinConfig Config{ get; set; }

        public AppSettings()
        {
            Favourites = new List<MarketFavourite>();
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
