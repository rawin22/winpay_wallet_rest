using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using Wallet.Interfaces;

namespace Wallet.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly string _senderEmail;
    private readonly string _recipientEmail;
    private readonly List<string> _supportRecipientEmails;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? string.Empty;
        _port = int.Parse(_configuration["EmailSettings:Port"] ?? string.Empty);
        _username = _configuration["EmailSettings:Username"] ?? string.Empty;
        _password = _configuration["EmailSettings:Password"] ?? string.Empty;
        _senderEmail = _configuration["EmailSettings:SenderEmail"] ?? string.Empty;
        _recipientEmail = _configuration["EmailSettings:RecipientEmail"] ?? string.Empty;
        _supportRecipientEmails = _configuration.GetSection("EmailSettings:SupportRecipientEmails").Get<List<string>>();

    }

    public async Task SendEmailAsync(string from, string to, string subject, string body, List<Attachment> attachments)
    {
        using (var client = new SmtpClient(_smtpServer))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_username, _password);
            client.EnableSsl = false;
            client.Port = _port;

            var message = new MailMessage
            {
                From = new MailAddress(_senderEmail),
                Subject = subject ?? string.Empty,
                Body = body ?? string.Empty
            };

            Console.WriteLine($"SendEmailAsync, to: {to} ");
            message.To.Add(to);

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            await client.SendMailAsync(message);
        }
    }

    public async Task SendSupportEmailAsync(string from, string subject, string body, List<Attachment>? attachments)
    {
        using (var client = new SmtpClient(_smtpServer))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_username, _password);
            client.EnableSsl = false;
            client.Port = _port;

            var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject ?? string.Empty,
                Body = body ?? string.Empty
            };

            Console.WriteLine($"SendEmailAsync, _supportRecipientEmails: {JsonConvert.SerializeObject(_supportRecipientEmails)} ");
            foreach (var recipient in _supportRecipientEmails)
            {
                message.To.Add(recipient);
            }

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }
            }

            await client.SendMailAsync(message);
        }
    }
}
