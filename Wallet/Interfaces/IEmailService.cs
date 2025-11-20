using System.Net.Mail;

namespace Wallet.Interfaces;

public interface IEmailService
{
	Task SendEmailAsync(string from, string to, string subject, string body, List<Attachment> attachments);
	Task SendSupportEmailAsync(string from, string subject, string body, List<Attachment> attachments);
}