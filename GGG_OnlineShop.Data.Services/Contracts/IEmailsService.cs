namespace GGG_OnlineShop.Data.Services.Contracts
{
    public interface IEmailsService
    {
        void SendEmail(string emailTo, string subject, string body, string smptClient, string fromMail, string password);
    }
}
