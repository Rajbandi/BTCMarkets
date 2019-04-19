using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class CreateOrderResponse
    {

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "errorCode")]
        public long? ErrorCode { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "id")]
        public long? Id { get; set; }

        [DataMember(Name = "clientRequestId")]
        public string ClientRequestId { get; set; }
    }

}
