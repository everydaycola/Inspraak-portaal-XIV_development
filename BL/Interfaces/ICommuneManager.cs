using Domain.CitizenPanel;
using UI_MVC.Models.Dto.communeDtos;

namespace BL.Interfaces;

public interface ICommuneManager
{
    public Task<List<Commune>> GetCommunes();
    public Task<List<CommuneBasicDto>> GetCommunesBasic();
}