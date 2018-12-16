namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.ComponentModel.DataAnnotations;

    public class OrderedItem: BaseModel<int>
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
    }
}
