using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class AppMessage 
    {
        public const string MarketAdded = "MarketAdded";
        public const string MarketUpdated = "MarketUpdated";
        public const string MarketRemoved = "MarketRemoved";
        public const string MarketFavouriteAdded = "MarketFavouriteAdded";
        public const string MarketFavouriteRemoved = "MarketFavouriteRemoved";

        public const string MarketsUpdated = "MarketsUpdated";

        public const string MarketTradeOrderAdded = "MarketTradeOrderAdded";
        public const string MarketTradeOrderUpdated = "MarketTradeOrderUpdated";
        public const string MarketTradeOrderRemoved = "MarketTradeOrderRemoved";



    }
}
