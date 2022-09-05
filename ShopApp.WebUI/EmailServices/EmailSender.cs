using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ShopApp.WebUI.EmailServices
{
    public class EmailSender : IEmailSender
    {

        private const string SendGridKey = "Your Api key";
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(SendGridKey, subject, htmlMessage, email);
        }

        private Task Execute(string sendGridkey, string subject, string message, string email)
        {
            //SendGridClient için paket yüklemesi yapmamız lazım
            var client = new SendGridClient(sendGridkey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("teslapower54@hotmail.com", "ShopApp"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent=message
            };

            msg.AddTo(new EmailAddress(email));

            //msg.AddBcc   mesajın kopyasının kime gideceği vs...

            return client.SendEmailAsync(msg);
        }
    }
}
