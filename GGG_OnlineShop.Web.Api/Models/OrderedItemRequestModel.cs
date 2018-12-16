namespace GGG_OnlineShop.Web.Api.Models
{
    using Common;
    using Infrastructure;
    using InternalApiDB.Models;
    using System.ComponentModel.DataAnnotations;

    public class OrderedItemRequestModel : IMapTo<OrderedItem>
    {
        [Required]
        [StringLength(GlobalConstants.ManufacturerMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Manufacturer { get; set; }

        [StringLength(GlobalConstants.EurocodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string EuroCode { get; set; }

        [StringLength(GlobalConstants.OtherCodesMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OtherCodes { get; set; }

        [Required]
        [StringLength(GlobalConstants.DescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        [Required]
        [Range(GlobalConstants.MinPrice, GlobalConstants.MaxPrice)]
        public double Price { get; set; }

        public override string ToString()
        {
            string info = $@"
Производител:        {Manufacturer}
EuroCode:            {EuroCode}
Други кодове:        {OtherCodes}
Описание:            {Description}
Цена:                {Price} лв
";
            return info;
        }
    }
}