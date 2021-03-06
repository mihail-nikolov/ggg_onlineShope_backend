﻿using System.Threading.Tasks;

namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using System.Net;
    using System.Net.Mail;

    public class EmailsService : IEmailsService
    {
        public async Task SendEmail(string emailTo, string subject, string body, string smptClient, string fromMail, string password)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(smptClient);
            mail.From = new MailAddress(fromMail);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new NetworkCredential(fromMail, password);
            SmtpServer.EnableSsl = true;
            await SmtpServer.SendMailAsync(mail);
        }
    }
}
