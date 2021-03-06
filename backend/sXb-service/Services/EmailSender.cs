﻿using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using sXb_service.Helpers;

namespace sXb_service.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }
        private readonly SMTPConfig smtpConfig;
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, SMTPConfig smtpConfig)
        {
            this.smtpConfig = smtpConfig;
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public void SendEmailAsync(string email, string subject, string message)
        {
            Execute(email, subject, message);
        }

        public void SendEmailAsync(string emailTo, string replyTo, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(emailTo));
                message.From = new MailAddress(smtpConfig.sendAddress);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.ReplyToList.Add(new MailAddress(replyTo));
                using (var client = new SmtpClient(smtpConfig.Host))
                {
                    string username = smtpConfig.Username;
                    string password = smtpConfig.Password;
                    client.Port = smtpConfig.Port;
                    client.Credentials = new NetworkCredential(username, password);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }

        private void Execute(string email, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress(smtpConfig.sendAddress);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (var client = new SmtpClient(smtpConfig.Host))
                {
                    string username = smtpConfig.Username;
                    string password = smtpConfig.Password;
                    client.Port = smtpConfig.Port;
                    client.Credentials = new NetworkCredential(username, password);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
    }
}