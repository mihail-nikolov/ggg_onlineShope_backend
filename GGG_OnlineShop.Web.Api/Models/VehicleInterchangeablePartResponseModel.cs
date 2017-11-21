namespace GGG_OnlineShop.Web.Api.Models
{
    using Infrastructure;
    using InternalApiDB.Models;

    public class VehicleInterchangeablePartResponseModel : IMapFrom<VehicleGlassInterchangeablePart>
    {
        public string Description { get; set; }

        public string EuroCode { get; set; }

        public string OesCode { get; set; }

        public string MaterialNumber { get; set; }

        public string LocalCode { get; set; }

        public string ScanCode { get; set; }

        public string NagsCode { get; set; }

        public string CleanEurocode
        {
            get
            {
                // todo constant for min eurocode length also for ;
                var eurocodeArray = this.EuroCode.Split(';');
                string cleanEurocode = string.Empty;

                if (eurocodeArray.Length > 0 && (eurocodeArray[0].Length >= 5 && eurocodeArray[0].Length <= 15))
                {
                    cleanEurocode = eurocodeArray[0];
                }

                return cleanEurocode;
            }
        }
    }
}