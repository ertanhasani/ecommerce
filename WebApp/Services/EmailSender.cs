using System.Net;
using System.Net.Mail;
using WebApp.Data;

namespace WebApp.Services;

public class EmailSender : IEmailSender
{
    private readonly eCommerceContext _context;

    public EmailSender(eCommerceContext context)
    {
        _context = context;
    }

    public Task SendEmailAsync(string email, string subject, string message)
    {
        var emailSetting = _context.SenderSettings.FirstOrDefault(x => x.IsEmail);

        if (emailSetting != null)
        {
            var client = new SmtpClient
            {
                Port = emailSetting.PortNumber ?? 0,
                Host = emailSetting.Server,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailSetting.Username, emailSetting.Password)
            };

            return client.SendMailAsync(emailSetting.From, email, subject, message);
        }

        return null;
    }
}