namespace BL.Interfaces;

public interface ISendMailManager
{
    public Task SendSingleQRCodeInMailAsync(string email, string data);
    public Task SendMultipleMails(IDictionary<string, string> emailAndData);
}