namespace GGG_OnlineShop.Web.Api.Models
{
    public class EnquiryRequestModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string ModelYear { get; set; }

        public string Body { get; set; }

        public string ProductType { get; set; }

        public bool EnquireToRuse { get; set; }

        public bool EnquireToSofia { get; set; }

        public string VIN { get; set; }

        public string Description { get; set; }
    }
}