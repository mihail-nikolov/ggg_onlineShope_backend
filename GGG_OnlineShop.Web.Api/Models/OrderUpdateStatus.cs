using System;

namespace GGG_OnlineShop.Web.Api.Models
{
    public class OrderUpdateStatus
    {
        public EpayStatus STATUS { get; set; }

        public string ENCODED { get; set; }

        public string CHECKSUM { get; set; }

        public int INVOICE { get; set; }

        public DateTime PAY_TIME { get; set; }

        public int STAN { get; set; }

        public string BCODE { get; set; }

        public string AMOUNT { get; set; }

        public string BIN { get; set; }
    }

    public enum EpayStatus
    {
        PAID, DENIED, EXPIRED
    }
}   