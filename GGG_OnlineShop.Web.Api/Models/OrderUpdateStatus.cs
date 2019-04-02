using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GGG_OnlineShop.Web.Api.Models
{
    public class OrderUpdateStatus
    {
        [Required]
        [JsonProperty("STATUS")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EpayStatus Status { get; set; }

        [JsonProperty("ENCODED")]
        public string Encoded { get; set; }

        [JsonProperty("CHECKSUM")]
        public string Checksum { get; set; }

        [JsonProperty("INVOICE")]
        public int Invoice { get; set; }

        [JsonProperty("PAY_TIME")]
        public DateTime PayTime { get; set; }

        [JsonProperty("STAN")]
        public int Stan { get; set; }

        [JsonProperty("BCODE")]
        public string Bcode { get; set; }

        [JsonProperty("AMOUNT")]
        public string Amount { get; set; }

        [JsonProperty("BIN")]
        public string Bin { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public enum EpayStatus
    {
        [EnumMember(Value = "PAID")]
        Paid,
        [EnumMember(Value = "DENIED")]
        Denied,
        [EnumMember(Value = "EXPIRED")]
        Expired
    }

    public enum ShopResponse
    {
        [EnumMember(Value = "OK")]
        Ok,
        [EnumMember(Value = "ERR")]
        Error,
        [EnumMember(Value = "NO")]
        NotFound
    }

    public class EpayResponse
    {
        [JsonProperty("INVOICE", NullValueHandling = NullValueHandling.Ignore)]
        public int? Invoice { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShopResponse Status { get; set; }

        [JsonProperty("err", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        [JsonProperty("errm", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }
    }
}