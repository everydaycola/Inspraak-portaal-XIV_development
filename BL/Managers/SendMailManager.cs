using BL.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BL.Managers;

public class SendMailManager : ISendMailManager
{
    private readonly ILogger<SendMailManager> _logger;
    private readonly IFileManager _fileManager;

    public SendMailManager(ILogger<SendMailManager> logger, IFileManager fileManager)
    {
        _logger = logger;
        _fileManager = fileManager;
    }

    public async Task SendSingleQrCodeInMailAsync(string email, string data)
    {
        var qrCodeBytes = _fileManager.CreateSingleQrCode(data);
        var base64QrCode = Convert.ToBase64String(qrCodeBytes);
        var imgSrc = $"data:image/png;base64,{base64QrCode}";

        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        var fromMail = new EmailAddress("ipveertien@outlook.com", "ipveertien");
        var subject = "Doe je mee aan ons burgerpanel?";
        var toMail = new EmailAddress(email, "Potentieel Panellid");

        var plainTextContent = "Wij zoeken mensen als jij!";
        var htmlContent = $@"<p>Wil je meedoen aan ons panel?</p>
                            <p><strong>Scan de QR-code hieronder:</strong></p>
                            <img src=""{imgSrc}"" alt=""QR code"" />
                            <p>Of klik hier: <a href=""{data}"">{data}</a></p>";


        var msg = MailHelper.CreateSingleEmail(fromMail, toMail, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Email sent to {email} successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to send email to {email}");
            Console.WriteLine($"StatusCode: {response.StatusCode}");
        }
    }

    public async Task SendSingleMailAsync(string email, string mailSubject, string textPart, string htmlPart)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        var fromMail = new EmailAddress("ipveertien@outlook.com", "ipveertien");
        var toMail = new EmailAddress(email, "Potentieel Panellid");

        var msg = MailHelper.CreateSingleEmail(fromMail, toMail, mailSubject, textPart, htmlPart);
        var response = await client.SendEmailAsync(msg);
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Email sent to {email} successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to send email to {email}");
            Console.WriteLine($"StatusCode: {response.StatusCode}");
        }
    }

    public async Task SendBulkMails(List<string> emails, string mailSubject, string textPart,
        string htmlPart)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);

        var fromMail = new EmailAddress("ipveertien@outlook.com", "ipveertien");
        var subject = mailSubject;

        var allEmails = new List<EmailAddress>();
        foreach (var email in emails)
        {
            allEmails.Add(new EmailAddress(email));
        }

        var plainTextContent = textPart;
        var htmlContent = htmlPart;

        if (!allEmails.IsNullOrEmpty())
        {
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(fromMail, allEmails, subject, plainTextContent,
                htmlContent);
            var response = await client.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"All emails have been sent successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to send emails");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
            }
        }
    }
}