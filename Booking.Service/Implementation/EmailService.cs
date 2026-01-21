using Booking.Domain.Email;
using Booking.Service.Interface;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public Boolean SendEmailAsync(EmailMessage allMails)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_mailSettings.EmailDisplayName, _mailSettings.SmtpUserName));
            emailMessage.To.Add(MailboxAddress.Parse(allMails.MailTo));
            emailMessage.Subject = allMails.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = allMails.Content
            };

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect(_mailSettings.SmtpServer, _mailSettings.SmtpServerPort, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.SmtpUserName, _mailSettings.SmtpPassword);
                    smtp.Send(emailMessage);
                    smtp.Disconnect(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email error: " + ex.Message);
                return false;
            }
        }

    }
}
