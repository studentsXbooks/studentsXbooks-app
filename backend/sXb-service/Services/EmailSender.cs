using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using System;
using Microsoft.Extensions.Configuration;

namespace sXb_service.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, IConfiguration configuration)
        {
            Options = optionsAccessor.Value;
            Configuration = configuration;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public void SendEmailAsync(string email, string subject, string message)
        {
            Execute(subject, message, email);
        }

        public void Execute(string subject, string body, string email)
        {
            string senderEmail = Configuration["SMTP:address"];
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress(senderEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    
                    string password = Configuration["SMTP:password"];
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(senderEmail, password);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
    }
}
