using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class OrderTradeHistoryResponse
    {

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "errorCode")]
        public int? ErrorCode { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "trades")]
        public IList<Trade> Trades { get; set; }
    }

}
