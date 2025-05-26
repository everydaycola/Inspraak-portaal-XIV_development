namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class AnswerOptionDto
{
    public string AnswerText { get; set; }
    public ICollection<AnswerOptionImpactsViewModel> Impacts { get; set; }
}