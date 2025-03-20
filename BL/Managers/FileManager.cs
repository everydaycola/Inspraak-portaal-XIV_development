using System.IO.Compression;
using BL.Generator;
using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.Extensions.Logging;

namespace BL.Managers;

public class FileManager : IFileManager
{
    private readonly QrCodeGenerator _qrCodeGenerator;
    private readonly ILogger<CriteriaManager> _logger;

    public FileManager(QrCodeGenerator qrCodeGenerator, ILogger<CriteriaManager> logger)
    {
        _qrCodeGenerator = qrCodeGenerator;
        _logger = logger;
    }

    public byte[] CreateZipFileForAllCodesInAGroup(CriteriaGroup criteriaGroup, string defaultUri)
    {
        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            AddGroupQrCodesToArchive(archive, criteriaGroup, defaultUri);
        }

        ms.Position = 0;
        return ms.ToArray();
    }

    public byte[] CreateSingleQrCode(string qrCodeData)
    {
        try
        {
            return _qrCodeGenerator.GenerateQrCode(qrCodeData);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Critical, "QRCode generator was called with empty data");
            return null;
        }
    }

    public byte[] CreateZipFileForAllCodesInAllGroups(IEnumerable<CriteriaGroup> criteriaGroups, string defaultUri)
    {
        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            foreach (var group in criteriaGroups)
            {
                AddGroupQrCodesToArchive(archive, group, defaultUri);
            }
        }

        ms.Position = 0;
        return ms.ToArray();
    }
    
    private void AddGroupQrCodesToArchive(ZipArchive archive, CriteriaGroup group, string defaultUri)
    {
        foreach (var member in group.PanelMembers)
        {
            var qrCodeBytes = _qrCodeGenerator.GenerateQrCode($"{defaultUri}?UserId={member.PanelMemberId.ToString()}");

            if (qrCodeBytes == null || qrCodeBytes.Length == 0)
            {
                throw new InvalidOperationException("Failed to generate QR code.");
            }
            var entry = archive.CreateEntry($"{group.Name}/qrcode_{member.PanelMemberId}.png");
            using var entryStream = entry.Open();
            entryStream.Write(qrCodeBytes, 0, qrCodeBytes.Length);
        }
    }
}