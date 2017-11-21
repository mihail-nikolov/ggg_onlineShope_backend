namespace GGG_OnlineShop.Common.Services
{
    using Contracts;
    using System.IO;

    public class FileLogger : ILogger
    {
        public FileLogger()
        {
        }

        public void LogError(string errorMessage, string placeToWriteErrors)
        {
            using (StreamWriter sw = File.AppendText(placeToWriteErrors))
            {
                sw.WriteLine(errorMessage);
            }
        }

        public void LogInfo(string info, string placeToWriteInfo)
        {
            using (StreamWriter sw = File.AppendText(placeToWriteInfo))
            {
                sw.WriteLine(info);
            }
        }
    }
}
