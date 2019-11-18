using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Services
{
    public interface IEmailSender
    {
        void SendEmailAsync(string email, string subject, string message);

        void SendEmailAsync(string emailTo, string replyTo, string subject, string message);
    }
}
