namespace ProductExcelSheets.app.tests.Mocked
{
    using GGG_OnlineShop.Common.Services.Contracts;
    using System.Collections.Generic;

    public class MockedReader : IReader
    {
        private Queue<string> arguments;

        public MockedReader(Queue<string> arguments)
        {
            this.arguments = arguments;
        }
        public string Read(string placeToReadFrom = "")
        {
            return this.arguments.Dequeue();
        }
    }
}
