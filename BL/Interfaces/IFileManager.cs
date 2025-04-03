using System.IO.Compression;
using BL.Generator;
using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface IFileManager
{
    public byte[] CreateSingleQrCode(string qrCodeData);
    public byte[] CreateZipFileForMultiplePanelMembers(IEnumerable<PanelMember> members, string defaultUri);
}