namespace UI_MVC.Models.Dto;

public class CriteriaDto
{
    public string Name { get; set; }
    public string Question { get; set; }
    public bool IsDefault { get; set; }
    public ICollection<CriteriaAnswerOptionDto> AnswerOptions { get; set; } = new List<CriteriaAnswerOptionDto>();
}