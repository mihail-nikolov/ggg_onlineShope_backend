namespace ProductExcelSheets.app
{
    using GGG_OnlineShop.Common.Services.Contracts;
    using System;

    class ConsoleWriter : ILogger
    {
        public void LogError(string errorMessage, string placeToWriteErrors = "")
        {
            Console.WriteLine($@"[Error]: {errorMessage}");
        }

        public void LogInfo(string info, string placeToWriteInfo = "")
        {
            Console.WriteLine(info);
        }
    }
}
