using BL.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

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
    public async Task SendSingleQRCodeInMailAsync(string email, string data)
    {
        var qrCodeBytes = _fileManager.CreateSingleQrCode(data);
        var base64QrCode = Convert.ToBase64String(qrCodeBytes);
        var imgSrc = $"data:image/png;base64,{base64QrCode}";

        var client =
            new MailjetClient(Environment.GetEnvironmentVariable("MJ_APIKEY_PUBLIC"),
                Environment.GetEnvironmentVariable("MJ_APIKEY_PRIVATE"));

        var request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "ipveertien@outlook.com")
            .Property(Send.FromName, "ipveertien")
            .Property(Send.Subject, "Doe je mee aan ons burgerpanel?")
            .Property(Send.TextPart, "Wij zoeken mensen zoals jou!")
            .Property(Send.HtmlPart, $@"
                <h3>Wil je meedoen aan ons panel?</h3>
                <p><strong>Scan de QR-code hieronder:</strong></p>
                <img src='{imgSrc}' alt='QR code' />
                <p>Of klik hier: <a href='{data}'>{data}</a></p>
            ").Property(Send.Recipients, new JArray
            {
                new JObject
                {
                    { "Email", email }
                }
            });

        MailjetResponse response = await client.PostAsync(request);
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Email sent to {email} successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to send email to {email}");
            Console.WriteLine($"StatusCode: {response.StatusCode}");
            Console.WriteLine($"ErrorInfo: {response.GetErrorInfo()}");
            Console.WriteLine($"ErrorMessage: {response.GetErrorMessage()}");
        }
    }

    public async Task SendMultipleMails(IDictionary<string, string> emailAndData)
    {
        foreach (var pair in emailAndData)
        {
            await SendSingleQRCodeInMailAsync(pair.Key, pair.Value);
        }
    }
}