namespace GGG_OnlineShop.Web.Api.Areas.Administration.Models.Users
{
    using Api.Models;
    using System;

    public class UserResponseModel : AccountInfoResponseModel
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsCompany { get; set; }

        public bool OnlyHighCostVisible { get; set; }
    }
}