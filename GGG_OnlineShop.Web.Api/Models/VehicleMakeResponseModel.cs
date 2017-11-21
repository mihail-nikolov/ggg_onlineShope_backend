namespace GGG_OnlineShop.Web.Api.Models
{
    using InternalApiDB.Models;
    using Infrastructure;

    public class VehicleMakeResponseModel : IMapFrom<VehicleMake>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}