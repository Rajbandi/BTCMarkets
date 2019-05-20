using BtcMarkets.Core.Helpers;
using BtcMarkets.Wallet.Helpers;
using System.Runtime.Serialization;

namespace BtcMarkets.Wallet.Models
{
    public class FundTransferData
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }

        [DataMember(Name = "lastupdate")]
        public long LastUpdate { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "amount")]
        public double Amount { get; set; }

        [DataMember(Name = "fee")]
        public double Fee { get; set; }

        [DataMember(Name = "txid")]
        public string TxId { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        public string AmountString
        {
            get
            {
                var value = AppHelper.FormatNumber(Amount, Currency);
                return value;
            }
        }
        public string FeeString
        {
            get
            {
                var value = AppHelper.FormatNumber(Fee, Currency);
                return value;
            }
        }
        public string DateString
        {
            get
            {
                var value = ApiHelper.ToLocalTime(Timestamp).ToString("dd-MMM-yyyy HH:mm");
                return value;
            }
        }

        public string LastUpdatedString
        {
            get
            {
                var value = ApiHelper.ToLocalTime(LastUpdate).ToString("dd-MMM-yyyy HH:mm");
                return value;
            }
        }
    }
}
