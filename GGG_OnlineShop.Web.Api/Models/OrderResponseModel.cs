using System;
using System.Collections.Generic;
using GGG_OnlineShop.Infrastructure;
using GGG_OnlineShop.InternalApiDB.Models;
using GGG_OnlineShop.InternalApiDB.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GGG_OnlineShop.Web.Api.Models
{
    public class OrderResponseModel : IMapFrom<Order>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("Status", Required = Required.Always)]
        public DeliveryStatus Status { get; set; }

        public string FullAddress { get; set; }

        public bool IsInvoiceNeeded { get; set; }

        public bool WithInstallation { get; set; }

        public string DeliveryNotes { get; set; }

        public double Price { get; set; }

        public double PaidPrice { get; set; }

        public double DiscountPercentage { get; set; }

        public List<OrderedItemResponseModel> OrderedItems { get; set; }
    }
}