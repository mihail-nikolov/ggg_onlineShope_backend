namespace GGG_OnlineShop.InternalApiDB.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Enums;
    using Base;

    public class Flag : BaseModel<int>
    {
        [Required]
        public string Name
        {
            get => FlagTypeEnum.ToString();
            set => FlagTypeEnum = value.ParseEnum<FlagType>();
        }

        public FlagType FlagTypeEnum { get; set; }

        public bool Value { get; set; }
    }
}
