using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    [DataContract]
    public class CryptoPaymentDetail
    {

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "txId")]
        public string TxId { get; set; }
    }

    [DataContract]
    public class FundTransfer
    {

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "fundTransferId")]
        public long FundTransferId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "creationTime")]
        public long CreationTime { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "amount")]
        public long? Amount { get; set; }

        [DataMember(Name = "fee")]
        public long? Fee { get; set; }

        [DataMember(Name = "transferType")]
        public string TransferType { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "lastUpdate")]
        public long LastUpdate { get; set; }

        [DataMember(Name = "cryptoPaymentDetail")]
        public CryptoPaymentDetail CryptoPaymentDetail { get; set; }
    }

    [DataContract]
    public class FundTransferResponse
    {

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "errorCode")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "fundTransfers")]
        public IList<FundTransfer> FundTransfers { get; set; }
    }

}
