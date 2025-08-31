using PetProject.Application.Services.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace PetProject.Application.Services.Impl;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }
    
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        
        var bodyBuilder = new BodyBuilder();

        bodyBuilder.TextBody = body;
        
        email.Body = bodyBuilder.ToMessageBody();

        using (var smtp = new SmtpClient())
        {
            try
            {
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                Console.WriteLine("Письмо успешно отправлено!");
            }
            catch
            {
                Console.WriteLine("Письмо не потравилось хз почему");
            }
        }
    }
}