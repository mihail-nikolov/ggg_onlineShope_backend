namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using InternalApiDB.Models.Enums;

    public class OrderedItemResponseModel : IMapFrom<OrderedItem>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Manufacturer { get; set; }

        public string EuroCode { get; set; }

        public string OtherCodes { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("Status", Required = Required.Always)]
        public DeliveryStatus Status { get; set; }

        public string FullAddress { get; set; }

        public bool IsInvoiceNeeded { get; set; }

        public bool WithInstallation { get; set; }

        public string DeliveryNotes { get; set; }

        public double Price { get; set; }

        //public bool IsDepositNeeded { get; set; }

        public double PaidPrice { get; set; }
    }
}