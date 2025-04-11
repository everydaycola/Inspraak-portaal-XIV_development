namespace BL.Interfaces;

public interface ISendMailManager
{
    Task SendSingleQRCodeInMailAsync(string email, string data);
    Task SendMultipleMails(IDictionary<string, string> emailAndData);
}