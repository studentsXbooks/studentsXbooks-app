using System.Collections.Generic;

namespace sXb_service.Helpers
{
    public class DatabaseConfig
    {
        public string Connection { get; set; }
    }

    public class CorsConfig
    {
        public string[] AllowedDomains { get; set; }
    }

    public class SMTPConfig
    {
        public string Username { get; set; }
        public string Host { get; set; }
        public string sendAddress { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}