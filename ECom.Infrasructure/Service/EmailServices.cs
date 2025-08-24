using ECom.Core.DTO;
using ECom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrasructure.Service
{
    public class EmailServices : IEmailServices
    {
        //SMTP
        private readonly IConfiguration configuration;

        public EmailServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(EmailDTO emailDTO)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Joe-Ecom",configuration["EmailSetting:From"])); 
            message.Subject = emailDTO.Subject;
            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(
                        configuration["EmailSetting:Smtp"],
                        int.Parse(configuration["EmailSetting:Port"]),true);
                    await smtp.AuthenticateAsync(configuration["EmailSetting:Username"], configuration["EmailSetting:Password"]);
                    await smtp.SendAsync(message);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("⛔ SMTP ERROR: " + ex.Message);
                    throw;
                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }
        }
    }
}
