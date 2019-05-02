using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class TradingFee : BaseResponse
    {

        [DataMember(Name = "tradingFeeRate")]
        public int TradingFeeRate { get; set; }

        [DataMember(Name = "volume30Day")]
        public long Volume30Day { get; set; }
    }

}
