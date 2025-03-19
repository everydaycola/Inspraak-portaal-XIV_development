using QRCoder;

namespace BL.Generator;

public class QrCodeGenerator
{
    public byte[] GenerateQrCode(string data, int pixelsPerModule = 20)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

        using var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(pixelsPerModule);
    }
}