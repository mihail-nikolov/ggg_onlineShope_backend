namespace GGG_OnlineShop.Common.Services
{
    using Contracts;
    using System.IO;

    public class FileReader : IReader
    {
        public FileReader() { }

        public string Read(string placeToReadFrom)
        {
            string fileContent = string.Empty;

            if (File.Exists(placeToReadFrom))
            {
                fileContent = File.ReadAllText(placeToReadFrom);
            }

            return fileContent;
        }
    }
}