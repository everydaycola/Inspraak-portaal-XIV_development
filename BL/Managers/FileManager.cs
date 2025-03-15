using System.IO.Compression;
using BL.Generator;
using BL.Interfaces;
using Domain.CitizenPanel;

namespace BL.Managers;

public class FileManager : IFileManager
{
    private readonly QrCodeGenerator _qrCodeGenerator;

    public FileManager(QrCodeGenerator qrCodeGenerator)
    {
        this._qrCodeGenerator = qrCodeGenerator;
    }

    public byte[] CreateZipFileForAllCodesInAGroup(CriteriaGroup criteriaGroup, string defaultUri)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                AddGroupQrCodesToArchive(archive, criteriaGroup, defaultUri);
            }

            ms.Position = 0;
            return ms.ToArray();
        }
    }

    public byte[] CreateSingleQrCode(string qrCodeData)
    {
        return _qrCodeGenerator.GenerateQrCode(qrCodeData);
    }

    public byte[] CreateZipFileForAllCodesInAllGroups(IEnumerable<CriteriaGroup> criteriaGroups, string defaultUri)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var group in criteriaGroups)
                {
                    AddGroupQrCodesToArchive(archive, group, defaultUri);
                }
            }

            ms.Position = 0;
            return ms.ToArray();
        }
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
            using (var entryStream = entry.Open())
            {
                entryStream.Write(qrCodeBytes, 0, qrCodeBytes.Length);
            }
        }
    }
}