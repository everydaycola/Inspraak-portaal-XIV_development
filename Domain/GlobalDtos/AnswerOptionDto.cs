namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class AnswerOptionDto
{
    public int Id { get; set; }
    public string AnswerText { get; set; }
    public List<Domain.GlobalDtos.AnswerOptionImpactsDto> Impacts { get; set; }
}