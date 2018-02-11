namespace GGG_OnlineShop.InternalApiDB.Models
{
    using Base;
    using Common;
    using Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Log : BaseModel<int>
    {
        [Required]
        [StringLength(GlobalConstants.LogPlaceMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.LogPlaceMinLength)]
        public string Place { get; set; }

        [Required]
        [StringLength(GlobalConstants.LogTypeMaxLength, ErrorMessage = GlobalConstants.MinAndMaxLengthErrorMessage, MinimumLength = GlobalConstants.LogTypeMinLength)]
        [Column("Type")]
        public string TypeString
        {
            get { return Type.ToString(); }
            private set { Type = value.ParseEnum<LogType>(); }
        }

        [StringLength(GlobalConstants.LogInfoMaxLength, ErrorMessage = GlobalConstants.MaxLengthErrorMessage)]
        public string Info { get; set; }

        public string Comment { get; set; }

        [NotMapped]
        public LogType Type { get; set; }
    }
}
