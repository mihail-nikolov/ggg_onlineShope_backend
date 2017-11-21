namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;

    public class VehicleModelResponseModel : IMapFrom<VehicleModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            VehicleModelResponseModel model = obj as VehicleModelResponseModel;
            if (model == null)
            {
                return false;
            }

            return model.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}