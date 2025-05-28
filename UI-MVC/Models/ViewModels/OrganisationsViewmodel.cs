using DAL;
using Domain.Tenant;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;
namespace UI_MVC.Models.ViewModels;

public class OrganisationsViewmodel
{
    public IEnumerable<Organisation> Organisations { get; set; }
    public int AmountOfOrganisations { get; set; }
    public List<QuestionsViewModel> Questions { get; set; } = new List<QuestionsViewModel>();
    public QuestionsViewModel QuestionToEdit { get; set; }
}