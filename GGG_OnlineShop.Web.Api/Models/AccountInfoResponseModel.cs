namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;

    public class AccountInfoResponseModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Bulstat { get; set; }

        public bool IsDeferredPaymentAllowed { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string DeliveryCountry { get; set; }

        public string DeliveryTown { get; set; }

        public string DeliveryAddress { get; set; }

        public double PercentageReduction { get; set; }
    }
}