﻿namespace GGG_OnlineShop.Web.Api.Areas.Administration.Models.OrderedItems
{
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class OrderedItemRequestUpdateStatusModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DeliveryStatus Status{ get; set; }

        public override string ToString()
        {
            string status = string.Empty;
            if (Status == DeliveryStatus.New)
            {
                status = "Нова";
            }
            else if (Status == DeliveryStatus.Ordered)
            {
                status = "На път";
            }
            else if (Status == DeliveryStatus.Done)
            {
                status = "завършена";
            }

            string info = $@"
ID:     {Id}
Статус: {status}
";
            return info;
        }
    }
}