namespace GGG_OnlineShop.Common.Services.Contracts
{
    public interface IReader
    {
        // in case of console - empty string for place to read
        string Read(string placeToReadFrom="");
    }
}
