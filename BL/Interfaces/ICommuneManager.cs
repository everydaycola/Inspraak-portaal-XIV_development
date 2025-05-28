using Domain.GlobalDtos;

namespace BL.Interfaces;

public interface ICommuneManager
{
    public Task<List<Commune>> GetCommunes();
    public Task<List<CommuneBasicDto>> GetCommunesBasic();
}