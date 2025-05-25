namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class AnswerOptionViewModel
{
    public string AnswerText { get; set; }
    public ICollection<AnswerOptionViewModel> AnswerOptions { get; set; }
}