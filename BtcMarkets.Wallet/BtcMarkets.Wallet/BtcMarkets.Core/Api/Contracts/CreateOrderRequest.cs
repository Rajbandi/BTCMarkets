﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class CreateOrderRequest
    {

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [DataMember(Name = "price")]
        public long Price { get; set; }

        [DataMember(Name = "volume")]
        public long Volume { get; set; }

        [DataMember(Name = "orderSide")]
        public string OrderSide { get; set; }

        [DataMember(Name = "ordertype")]
        public string Ordertype { get; set; }

        [DataMember(Name = "clientRequestId")]
        public string ClientRequestId { get; set; }
    }

}
