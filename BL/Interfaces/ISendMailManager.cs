namespace BL.Interfaces;

public interface ISendMailManager
{
    public Task SendSingleQrCodeInMailAsync(string email, string data);
    public Task SendBulkMails(List<string> emails, string mailSubject, string textPart, string htmlPart);
    public Task SendSingleMailAsync(string email, string mailSubject, string textPart, string htmlPart);
}