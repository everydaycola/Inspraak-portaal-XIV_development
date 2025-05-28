using Domain.GlobalDtos;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class QuestionsViewModel
{
    public int Id { get; set; }
    public string Question { get; set; }
    public List<AnswerOptionDto> AnswerOptions { get; set; }
}