﻿namespace GGG_OnlineShop.Web.Api.Areas.Administration.Models.OrderedItems
{
    using Common;
    using InternalApiDB.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class OrderRequestUpdateStatusModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DeliveryStatus Status{ get; set; }

        public override string ToString()
        {
            string status = EnglishBulgarianDictionary.Namings[Status.ToString()];

            string info = $@"
ID: {Id}<br>
Статус: {status}
";
            return info;
        }
    }
}