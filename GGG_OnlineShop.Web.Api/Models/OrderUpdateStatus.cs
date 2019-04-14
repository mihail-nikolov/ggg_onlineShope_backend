namespace GGG_OnlineShop.Web.Api.Models
{
    public class OrderUpdateStatus
    {
        public EpayStatus Status { get; set; }

        public int Invoice { get; set; }
    }

    public enum EpayStatus
    {
        Paid,
        Denied,
        Expired
    }

    public enum ShopResponse
    {
        OK,
        ERR
    }
}