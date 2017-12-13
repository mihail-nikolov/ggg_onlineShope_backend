namespace GGG_OnlineShop.Data.Services.ExternalDb.Models
{
    public class GlassInfoResponseModel
    {
        public int Quantity { get; set; }

        public string Manufacturer { get; set; }

        public string DescriptionWithName { get; set; }

        public string DescriptionWithoutName { get; set; }

        public double? Price { get; set; }
    }
}
