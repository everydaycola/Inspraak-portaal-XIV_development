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

    public byte[] CreateZipFileForMultiplePanelMembers(IEnumerable<PanelMember> members, string defaultUri)
    {
        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            foreach (var member in members)
            {
                AddGroupQrCodesToArchive(archive, member, defaultUri);
            }
        }

        ms.Position = 0;
        return ms.ToArray();
    }
    
    private void AddGroupQrCodesToArchive(ZipArchive archive, PanelMember member, string defaultUri)
    {
        var qrCodeBytes = _qrCodeGenerator.GenerateQrCode($"{defaultUri}?UserId={member.PanelMemberId.ToString()}");
        var memberCriteriaGroupName = member.Responses;
        var groupName = string.Join("-", member.Responses.Select(r => r.SelectedOption));
        var entry = archive.CreateEntry($"{groupName}/qrcode_{member.PanelMemberId}.png");
        using var entryStream = entry.Open();
        entryStream.Write(qrCodeBytes, 0, qrCodeBytes.Length);
    }
}