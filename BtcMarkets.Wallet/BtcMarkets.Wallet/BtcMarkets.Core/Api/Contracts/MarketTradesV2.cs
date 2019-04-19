using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
   
    [DataContract]
    public class Paging :BaseObject
    {

        [DataMember(Name = "newer")]
        public string Newer { get; set; }

        [DataMember(Name = "older")]
        public string Older { get; set; }
    }

    [DataContract]
    public class MarketTradesV2 :BaseObject
    {

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "errorCode")]
        public object ErrorCode { get; set; }

        [DataMember(Name = "errorMessage")]
        public object ErrorMessage { get; set; }

        [DataMember(Name = "trades")]
        public IList<Trade> Trades { get; set; }

        [DataMember(Name = "paging")]
        public Paging Paging { get; set; }

    }

}
