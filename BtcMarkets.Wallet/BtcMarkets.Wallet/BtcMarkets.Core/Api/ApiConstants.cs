using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core.Api
{
    public static class ApiConstants
    {
        public const string ApiKey = "apikey";
        public const string TimeStamp = "timestamp";
        public const string Signature = "signature";

        public const string Btc = "BTC";
        public const string Aud = "AUD";
        public const string Eth = "ETH";
        public const string Powr = "POWR";


        public const int SECOND = 1;
        public const int MINUTE = 60 * SECOND;
        public const int HOUR = 60 * MINUTE;
        public const int DAY = 24 * HOUR;
        public const int MONTH = 30 * DAY;

        public const double CurrencyDecimal = 100000000;
        public const int TimestampDenom = 1000;
        public static string[] Currencies => new[] { Btc, Aud }; 
    }
}
