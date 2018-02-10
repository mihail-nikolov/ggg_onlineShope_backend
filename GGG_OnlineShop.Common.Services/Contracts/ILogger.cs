namespace GGG_OnlineShop.Common.Services.Contracts
{
    public interface ILogger
    {
        // in case of console - empty string for place to write

        void LogError(string error, string placeToWriteErrors = "");

        void LogInfo(string info, string placeToWriteInfo = "");
    }
}
