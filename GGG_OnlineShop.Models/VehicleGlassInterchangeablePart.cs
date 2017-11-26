namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassInterchangeablePart: BaseModel<int>
    {
        private ICollection<VehicleGlass> vehicleGlasses;

        public VehicleGlassInterchangeablePart()
        {
            this.vehicleGlasses = new HashSet<VehicleGlass>();
        }

        [StringLength(GlobalConstants.InterchangeableEurocodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string EuroCode { get; set; }

        [StringLength(GlobalConstants.OesCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OesCode { get; set; }

        [StringLength(GlobalConstants.LocalCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string LocalCode { get; set; }

        [StringLength(GlobalConstants.MaterialNumberMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string MaterialNumber { get; set; }

        [StringLength(GlobalConstants.ScanCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string ScanCode { get; set; }

        [StringLength(GlobalConstants.NagsCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string NagsCode { get; set; }

        [StringLength(GlobalConstants.DescriptionMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Description { get; set; }

        public virtual ICollection<VehicleGlass> VehicleGlasses
        {
            get
            {
                return this.vehicleGlasses;
            }
            set
            {
                this.vehicleGlasses = value;
            }
        }
    }
}