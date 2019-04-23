using System.Runtime.Serialization;

namespace GGG_OnlineShop.InternalApiDB.Models.Enums
{
    public enum PaymentMethod
    {
        [EnumMember(Value = "cash-on-delivery")]
        CashOnDelivery,
        [EnumMember(Value = "epay")]
        OnlinePayment,
    }
}
