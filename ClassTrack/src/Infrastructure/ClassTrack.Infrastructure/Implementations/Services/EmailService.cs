using ClassTrack.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Persistance.Implementations.Services
{
    internal class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string destination,string subject,string body)
        {
            SmtpClient smtpClient = new SmtpClient(_configuration["Email:Host"],
                                        Convert.ToInt32(_configuration["Email:Port"]));

            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_configuration["Email:LoginEmail"],
                                                           _configuration["Email:Password"]);

            MailAddress from = new MailAddress(_configuration["Email:LoginEmail"],"ClassTrack");
            MailAddress to = new MailAddress(destination);

            MailMessage message = new MailMessage(from,to);
            message.Subject = subject;
            message.Body = body;

            await smtpClient.SendMailAsync(message);

        }
    }
}
