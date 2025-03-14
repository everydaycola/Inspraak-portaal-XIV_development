using QRCoder;

namespace BL.Generator;

public class QrCodeGenerator
{
    
    public QrCodeGenerator()
    {
    }
    
    public byte[] GenerateQrCode(string data, int pixelsPerModule=20)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
    
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            return qrCode.GetGraphic(pixelsPerModule);
        }
    }
}