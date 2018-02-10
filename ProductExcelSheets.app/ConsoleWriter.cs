namespace ProductExcelSheets.app
{
    using GGG_OnlineShop.Common.Services.Contracts;
    using System;

    class ConsoleWriter : ILogger
    {
        public void LogError(string error, string placeToWriteErrors = "")
        {
            Console.WriteLine($@"[Error]: {error}");
        }

        public void LogInfo(string info, string placeToWriteInfo = "")
        {
            Console.WriteLine(info);
        }
    }
}
