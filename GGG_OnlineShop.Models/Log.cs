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
        public string Place { get; set; }

        [Required]
        [Column("Type")]
        public string TypeString
        {
            get { return Type.ToString(); }
            private set { Type = value.ParseEnum<LogType>(); }
        }

        [Required]
        public string Info { get; set; }

        public string Comment { get; set; }

        [NotMapped]
        public LogType Type { get; set; }
    }
}
