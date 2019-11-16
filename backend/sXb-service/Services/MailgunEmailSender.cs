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
    public class MailgunEmailSender : IEmailSender {
        public IConfiguration Configuration { get; }
        private readonly SMTPConfig smtpConfig;
        public MailgunEmailSender (IOptions<AuthMessageSenderOptions> optionsAccessor, SMTPConfig smtpConfig) {
            this.smtpConfig = smtpConfig;
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public void SendEmailAsync (string emailTo, string subject, string message) {
            SendRestSharpMessage (emailTo, subject, message);
        }
        public void SendEmailAsync (string emailTo, string replyTo, string subject, string message) {

            SendRestSharpMessage (emailTo, replyTo, subject, message);
        }
        public static IRestResponse SendRestSharpMessage (string emailTo, string subject, string message) {
            RestClient client = new RestClient ();
            client.BaseUrl = new Uri ("https://api.mailgun.net/v3");

            client.Authenticator = new HttpBasicAuthenticator ("api", "// input api key");
            RestRequest request = new RestRequest ();
            request.AddParameter ("domain", "mg.viasof.com", ParameterType.UrlSegment);
            request.Resource = "mg.viasof.com/messages";
            request.AddParameter ("from", "Excited User <mailgun@mg.viasof.com>");
            request.AddParameter ("to", emailTo);
            request.AddParameter ("subject", subject);
            request.AddParameter ("text", message);
            request.Method = Method.POST;
            return client.Execute (request);
        }
        public static IRestResponse SendRestSharpMessage (string emailTo, string replyTo, string subject, string message) {
            RestClient client = new RestClient ();
            client.BaseUrl = new Uri ("https://api.mailgun.net/v3");

            client.Authenticator = new HttpBasicAuthenticator ("api", "// input api key");
            RestRequest request = new RestRequest ();
            request.AddParameter ("domain", "mg.viasof.com", ParameterType.UrlSegment);
            request.Resource = "mg.viasof.com/messages";
            request.AddParameter ("from", "Student X Books<mailgun@mg.viasof.com>");
            request.AddParameter ("to", emailTo);
            request.AddParameter ("h:Reply-To", replyTo);
            request.AddParameter ("subject", subject);
            request.AddParameter ("text", message);
            request.Method = Method.POST;
            return client.Execute (request);
        }
    }
}