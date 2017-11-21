namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;

    public class VehicleBodyTypeResponseModel : IMapFrom<VehicleBodyType>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }
}