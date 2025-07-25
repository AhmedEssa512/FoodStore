using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FoodStore.Contracts.Interfaces;
using FoodStore.Services.Models;
using Microsoft.Extensions.Options;

namespace FoodStore.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _settings;

        public EmailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.From, _settings.Password),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(_settings.From, _settings.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            await client.SendMailAsync(message);

        }
    }
}