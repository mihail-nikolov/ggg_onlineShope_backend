namespace GGG_OnlineShop.InternalApiDB.Models.Base
{
    using System;

    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
