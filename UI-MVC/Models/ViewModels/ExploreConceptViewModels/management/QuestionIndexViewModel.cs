
namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels.management;

public class QuestionIndexViewModel
{
    public List<QuestionsViewModel> Questions { get; set; } = new List<QuestionsViewModel>();
    
    public IEnumerable<ParticipationViewModel> ParticipationMethods { get; set; }
}