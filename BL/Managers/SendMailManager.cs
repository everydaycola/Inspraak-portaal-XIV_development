using BL.Interfaces;
using DAL.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BL.Managers;

public class SendMailManager : ISendMailManager
{
    private readonly IFileManager _fileManager;

    public SendMailManager(IFileManager fileManager)
    {
        _fileManager = fileManager;
    }

    public async Task SendSingleQRCodeInMailAsync(string email, string data)
    {
        var qrCodeBytes = _fileManager.CreateSingleQrCode(data);

        var base64QrCode = Convert.ToBase64String(qrCodeBytes);
        var imgSrc = $"data:image/png;base64,{base64QrCode}";
        
        // TODO deze api-key in een environment variabele zetten en dan ook in de cloud Secret Manager
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from_email = new EmailAddress("ipveertien@outlook.com", "test");
        var to_email = new EmailAddress(email, "Potentieel panellid");
        var subject = "Would you like to join our panel?";

        var plainTextContent = "Scan de QR-code in de bijlage of klik op de link om deel te nemen!";
        var htmlContent = $@"
        <p>Wil je meedoen aan ons panel?</p>
        <p><strong>Scan de QR-code hieronder:</strong></p>
        <img src=""{imgSrc}"" alt=""QR code"" />
        <p>Of klik hier: <a href=""{data}"">{data}</a></p>";
        var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, htmlContent);
        await client.SendEmailAsync(msg);
    }

    public async Task SendMultipleMails(IDictionary<string, string> emailAndData)
    {
        foreach (var pair in emailAndData)
        {
            await SendSingleQRCodeInMailAsync(pair.Key, pair.Value);
        }
    }
}