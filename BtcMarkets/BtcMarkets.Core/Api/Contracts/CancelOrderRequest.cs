using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class CancelOrderRequest
    {

        [DataMember(Name = "orderIds")]
        public IList<long> OrderIds { get; set; }
    }

}
