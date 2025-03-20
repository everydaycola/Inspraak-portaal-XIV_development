using QRCoder;

namespace BL.Generator;

public class QrCodeGenerator
{
    public byte[] GenerateQrCode(string data, int pixelsPerModule = 20)
    {
        if (data.Length == 0)
        {
            throw new ArgumentNullException("GenerateQrCode expects a valid non empty string.");
        }
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(pixelsPerModule);
    }
}