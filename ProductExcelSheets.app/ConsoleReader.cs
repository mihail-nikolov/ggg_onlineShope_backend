namespace ProductExcelSheets.app
{
    using GGG_OnlineShop.Common.Services.Contracts;
    using System;

    public class ConsoleReader : IReader
    {
        public string Read(string placeToReadFrom = "")
        {
            return Console.ReadLine();
        }
    }
}
