using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class TradingFee
    {

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "errorCode")]
        public object ErrorCode { get; set; }

        [DataMember(Name = "errorMessage")]
        public object ErrorMessage { get; set; }

        [DataMember(Name = "tradingFeeRate")]
        public int TradingFeeRate { get; set; }

        [DataMember(Name = "volume30Day")]
        public long Volume30Day { get; set; }
    }

}
