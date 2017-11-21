namespace ProductExcelSheets.app
{
    using GGG_OnlineShop.Common.Services.Contracts;

    public interface IExcelManager
    {
        ILogger fileLogger { get; set; }

        void AdaptDescription(string sheetName);

        void AdaptProductsPlace(string sourceSheet, string targetSheet, string interchangeablesSheetName);
        // could be in one method
        void ReplaceGivenColumnWithSourceOne(string sourceSheet, string targetSheet, string columnToReplace);

        void AddQuantities(string sourceSheet, string targetSheet);

        void AddInterchangeables(string sourceSheet, string targetSheet);

    }
}
