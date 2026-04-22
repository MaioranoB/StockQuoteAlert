using MailKit.Net.Smtp;
using MimeKit;
using StockQuoteAlert.Models;
using StockQuoteAlert.Settings;
using StockQuoteAlert.Templates;

namespace StockQuoteAlert.Services;

public class EmailService(ApplicationSettings appSettings)
{
    private readonly ApplicationSettings _appSettings = appSettings;

    public async Task SendStockAlertEmail(StockInfo stockResult, AlertType alertType, decimal threshold)
    {
        var (subject, body) = EmailTemplate.Build(stockResult, alertType, threshold);
        await SendEmailAsync(subject, body);
    }

    private async Task SendEmailAsync(string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_appSettings.Email.FromName, _appSettings.Email.FromAddress));
        message.To.Add(MailboxAddress.Parse(_appSettings.Email.RecipientAddress));

        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_appSettings.SMTP.Server, _appSettings.SMTP.Port, false);

        // Note: only needed if the SMTP server requires authentication
        await client.AuthenticateAsync(_appSettings.SMTP.User, _appSettings.SMTP.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
