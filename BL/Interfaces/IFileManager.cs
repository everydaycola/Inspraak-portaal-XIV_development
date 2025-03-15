using System.IO.Compression;
using BL.Generator;
using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface IFileManager
{
    public byte[] CreateZipFileForAllCodesInAllGroups(IEnumerable<CriteriaGroup> criteriaGroups,string defaultUri);
    public byte[] CreateZipFileForAllCodesInAGroup(CriteriaGroup criteriaGroup,string defaultUri);
    public byte[] CreateSingleQrCode(string qrCodeData);
}