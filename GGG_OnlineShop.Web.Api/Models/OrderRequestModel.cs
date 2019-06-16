using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GGG_OnlineShop.Common;
using GGG_OnlineShop.Infrastructure;
using GGG_OnlineShop.InternalApiDB.Models;
using GGG_OnlineShop.InternalApiDB.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GGG_OnlineShop.Web.Api.Models
{
    public class OrderRequestModel : IMapTo<Order>
    {
        [StringLength(GlobalConstants.FullAddressMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string FullAddress { get; set; }

        public DeliveryStatus Status { get; set; }

        [Required]
        [JsonProperty("WayToPay")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(GlobalConstants.DeliveryNotesMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string DeliveryNotes { get; set; }

        public bool WithInstallation { get; set; }

        public bool InstallationRuse { get; set; }

        public bool InstallationSofia { get; set; }
        // TODO  field for invoice

        [Required]
        public bool IsInvoiceNeeded { get; set; }

        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double PaidPrice { get; set; }

        [Required]
        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double Price { get; set; }

        public double DiscountPercentage { get; set; }

        [Required]
        [StringLength(GlobalConstants.UserЕmailMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string UserЕmail { get; set; }

        [Required]
        [StringLength(GlobalConstants.UserInfoMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string UserInfo { get; set; }

        public string UserId { get; set; }

        public List<OrderedItemRequestModel> OrderedItems { get; set; }

        public override string ToString()
        {
            string invoice = EnglishBulgarianDictionary.Namings[IsInvoiceNeeded.ToString()];
            string installation = EnglishBulgarianDictionary.Namings[WithInstallation.ToString()];
            string status = EnglishBulgarianDictionary.Namings[Status.ToString()];

            string info = "";
            foreach (var orderedItem in OrderedItems)
            {
                info += $"{orderedItem}<br>=======================<br>";
            }

            info += $@"
Пълен адрес: {FullAddress}<br>
Статус: {status}<br>
Бележки за доставка: {DeliveryNotes}<br>
Необходим монтаж: {installation}<br>
Необходима фактура: {invoice}<br><br>
Цена:{Price} лв<br>
Цена с отстъпка:{Price - DiscountPercentage/100*Price} лв<br>
Платено: {PaidPrice} лв<br>
";
            return info;
        }
    }
}