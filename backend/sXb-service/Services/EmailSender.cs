using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using sXb_service.Helpers;

namespace sXb_service.Services {
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender {
        public IConfiguration Configuration { get; }
        private readonly SMTPConfig smtpConfig;
        public EmailSender (IOptions<AuthMessageSenderOptions> optionsAccessor, SMTPConfig smtpConfig) {
            this.smtpConfig = smtpConfig;
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public void SendEmailAsync (string email, string subject, string message) {
            //Execute (subject, message, email);
            SendRestSharpMessage ();
        }
        public void SendEmailAsync(string emailTo, string replyTo, string subject, string message)
        {
            //Execute (subject, message, email);
            SendRestSharpMessage();
        }
        public void Execute (string subject, string body, string email) {
            var smtpConfig = Configuration.GetSection ("SMTP").Get<SMTPConfig> ();
            using (var message = new MailMessage ()) {
                message.To.Add (new MailAddress (email));
                message.From = new MailAddress (smtpConfig.sendAddress);

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                //message.ReplyToList.Add (new MailAddress (replyTo));
                using (var client = new SmtpClient (smtpConfig.Host)) {
                    string username = smtpConfig.Username;
                    string password = smtpConfig.Password;
                    client.Port = smtpConfig.Port;
                    client.Credentials = new NetworkCredential (username, password);
                    client.EnableSsl = true;
                    client.Send (message);
                }
            }
        }
        public async void SendSimpleMessage () {
            using (var client = new HttpClient {
                BaseAddress = new Uri ("https://api.mailgun.net/v3")
            }) {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue ("Basic",
                        Convert.ToBase64String (Encoding.ASCII.GetBytes (
                            "b3ebca3debf3a46899749c5aa4342827-f696beb4-b4af993a")));
                var content = new FormUrlEncodedContent (new [] {
                    new KeyValuePair<string, string> ("from", "Mailgun <postmaster@sandbox927c104d6d824a4486a07e0d03037b86.mailgun.org>"),
                        new KeyValuePair<string, string> ("to", "studentxbooks@gmail.com"),
                        new KeyValuePair<string, string> ("subject", "Hello Student X Books"),
                        new KeyValuePair<string, string> ("text", "Congratulations Student X Books, you just sent an email with Mailgun!  You are truly awesome!")
                });
                await client.PostAsync ("https://api.mailgun.net/v3/sandbox927c104d6d824a4486a07e0d03037b86.mailgun.org/messages", content).ConfigureAwait (false);
            }
        }
        public static IRestResponse SendRestSharpMessage () {
            RestClient client = new RestClient ();
            client.BaseUrl = new Uri ("https://api.mailgun.net/v3");

            client.Authenticator = new HttpBasicAuthenticator ("api", "b3ebca3debf3a46899749c5aa4342827-f696beb4-b4af993a");
            RestRequest request = new RestRequest ();
            request.AddParameter ("domain", "mg.viasof.com", ParameterType.UrlSegment);
            request.Resource = "mg.viasof.com/messages";
            request.AddParameter ("from", "Excited User <mailgun@mg.viasof.com>");
            request.AddParameter ("to", "studentxbooks@gmail.com");
            request.AddParameter ("subject", "Hello");
            request.AddParameter ("text", "Testing some Mailgun awesomness!");
            request.Method = Method.POST;
            return client.Execute (request);
        }
    }
}