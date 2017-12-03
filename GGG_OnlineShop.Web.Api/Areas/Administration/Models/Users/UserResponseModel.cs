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

        public bool IsSaintGobainVisible { get; set; }

        public bool IsPilkingtonVisible { get; set; }

        public bool IsYesglassVisible { get; set; }

        public bool IsNordglassVisible { get; set; }

        public bool IsLamexVisible { get; set; }

        public bool IsAGCVisible { get; set; }

        public bool IsFuyaoVisible { get; set; }

        public bool IsSharedVisible { get; set; }
    }
}