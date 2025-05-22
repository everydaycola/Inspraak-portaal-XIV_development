using DAL;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;

namespace UI_MVC.Models.Dto.OrganisationDtos;

public class OrganisationManagementDto
{
    public IEnumerable<Organisation> Organisations { get; set; }
    public int AmountOfOrganisations { get; set; }
    public List<QuestionManagementViewModel> Questions { get; set; } = new List<QuestionManagementViewModel>();
    public QuestionManagementViewModel QuestionToEdit { get; set; }
}