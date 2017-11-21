namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    public class ModelPartYearMonthJsonInfoModel
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public override string ToString()
        {
            string infoString = $@"{Month}/{Year}";

            return infoString;
        }
    }
}