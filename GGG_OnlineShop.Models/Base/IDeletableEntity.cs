namespace GGG_OnlineShop.InternalApiDB.Models.Base
{
    using System;

    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
