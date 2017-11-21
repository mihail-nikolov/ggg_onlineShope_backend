namespace GGG_OnlineShop.Data.Services.JsonParseModels
{
    public class ModelAndPartDatesJsonInfoModel
    {
        public ModelPartYearMonthJsonInfoModel From { get; set; }

        public ModelPartYearMonthJsonInfoModel To { get; set; }

        public override string ToString()
        {
            return $@"From: {From}; To: {To}";
        }
    }
}