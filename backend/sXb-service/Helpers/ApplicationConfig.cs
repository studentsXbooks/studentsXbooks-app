using System.Linq;

namespace sXb_service.Helpers
{
    public class DatabaseConfig
    {
        public string Connection { get; set; }
    }

    public class CorsConfig
    {
        public string FrontendDomain { get; set; }
        public string[] AllowedDomains { get; set; } = new string[0];

        public string[] AllDomains => AllowedDomains.Append(FrontendDomain).ToArray();
    }

    public class SMTPConfig
    {
        public string Username { get; set; }
        public string Host { get; set; }
        public string sendAddress { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }

    public class BookApiConfig
    {
        public string Apikey { get; set; }
    }
}