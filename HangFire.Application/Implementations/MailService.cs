using HangFire.Application.Contracts;
using HangFire.Data.Data;
using HangFire.Data.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HangFire.Application.Implementations
{
    public class MailService : IMailService
    {
        private readonly MailConfig _mailConfig;

        public MailService ( IOptions<MailConfig> mailConfig ) => _mailConfig = mailConfig.Value ?? throw new ArgumentNullException(nameof(MailConfig));
        public async Task SendAsync ( MailRequest request )
        {
            var message = CreateMessage(request);
            using var client = CreateSmtpClient();
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private MimeMessage CreateMessage ( MailRequest request )
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailConfig.SenderEmail , _mailConfig.SenderName));

            message.To.Add(new MailboxAddress(request.Recipient.Name , request.Recipient.Email));

            message.Headers.Add("Content-Type" , "text/html; charset=utf-8");

            message.Subject = request.Placeholders.Count > 0 ? ReplacePlaceholders(request.Subject , request.Placeholders) : request.Subject;

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = request.Placeholders.Count > 0 ? ReplacePlaceholders(request.Body , request.Placeholders) : request.Body
            };

            return message;
        }

        private SmtpClient CreateSmtpClient ()
        {
            var client = new SmtpClient();
            client.Connect(_mailConfig.Server , _mailConfig.Port , MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_mailConfig.UserName , _mailConfig.Password);
            return client;
        }

        public string ReplacePlaceholders ( string text , Dictionary<string , string> placeholders )
        {
            foreach ( var placeholder in placeholders )
            {
                text = text.Replace($"[{placeholder.Key}]" , placeholder.Value);
            }
            return text;
        }
    }
}
