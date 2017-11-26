namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class VehicleGlassSuperceed : BaseModel<int>
    {
        private ICollection<VehicleGlass> vehicleGlasses;

        public VehicleGlassSuperceed()
        {
            this.vehicleGlasses = new HashSet<VehicleGlass>();
        }

        [StringLength(GlobalConstants.MaterialNumberMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OldMaterialNumber { get; set; }

        [StringLength(GlobalConstants.OesCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OldOesCode { get; set; }

        [StringLength(GlobalConstants.EurocodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OldEuroCode { get; set; }

        [StringLength(GlobalConstants.LocalCodeMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string OldLocalCode { get; set; }

        [StringLength(GlobalConstants.SuperceedChangeDateMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string ChangeDate { get; set; }

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