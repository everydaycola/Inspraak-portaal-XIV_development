using System.Drawing;
using System.IO.Compression;
using QRCoder;
using UI_MVC.Models.Dto;

namespace UI_MVC.Models.Helper;

public static class FileHelper
{
    
    private static byte[] GenerateQRCode(string data)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
    
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            return qrCode.GetGraphic(20);
        }
    }
    
    public static byte[] CreateZipFileForAllCodesInAGroup(IEnumerable<GroupedUniqueCodesDto> dto)
    {
        if (dto == null || !dto.Any())
        {
            throw new ArgumentException("Input collection is empty.", nameof(dto));
        }

        using (MemoryStream ms = new MemoryStream())
        {
            using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var group in dto)
                {
                    if (group.Members == null || !group.Members.Any())
                    {
                        continue; // Skip empty groups
                    }

                    foreach (var member in group.Members)
                    {
                        var qrCodeBytes = GenerateQRCode($"http://localhost:5228{member.generatedUri}");
                        if (qrCodeBytes == null || qrCodeBytes.Length == 0)
                        {
                            throw new InvalidOperationException("Failed to generate QR code.");
                        }

                        var entry = archive.CreateEntry(group.Name + $"/qrcode_{member.memberId}.png");
                        using (var entryStream = entry.Open())
                        {
                            entryStream.Write(qrCodeBytes, 0, qrCodeBytes.Length);
                        }
                    }
                }
            }
            ms.Position = 0; // Reset the stream position to the beginning
            return ms.ToArray();
        }
    }


}