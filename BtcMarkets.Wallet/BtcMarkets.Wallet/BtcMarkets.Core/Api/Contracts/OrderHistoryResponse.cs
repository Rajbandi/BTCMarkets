using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class Trade
    {

        [DataMember(Name = "id")]
        public long? Id { get; set; }

        [DataMember(Name = "creationTime")]
        public long? CreationTime { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "price")]
        public long? Price { get; set; }

        [DataMember(Name = "volume")]
        public long? Volume { get; set; }

        [DataMember(Name = "fee")]
        public long? Fee { get; set; }

        [DataMember(Name = "side")]
        public string Side { get; set; }

        [DataMember(Name = "orderId")]
        public long? OrderId { get; set; }
    }

    [DataContract]
    public class Order
    {

        [DataMember(Name = "id")]
        public long? Id { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "instrument")]
        public string Instrument { get; set; }

        [DataMember(Name = "orderSide")]
        public string OrderSide { get; set; }

        [DataMember(Name = "ordertype")]
        public string Ordertype { get; set; }

        [DataMember(Name = "creationTime")]
        public string CreationTime { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "price")]
        public long? Price { get; set; }

        [DataMember(Name = "volume")]
        public long? Volume { get; set; }

        [DataMember(Name = "openVolume")]
        public long? OpenVolume { get; set; }

        [DataMember(Name = "clientRequestId")]
        public long? ClientRequestId { get; set; }

        [DataMember(Name = "trades")]
        public IList<Trade> Trades { get; set; }
        
    }

    [DataContract]
    public class OrderHistoryResponse
    {

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "errorCode")]
        public int? ErrorCode { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "orders")]
        public IList<Order> Orders { get; set; }
    }

}
