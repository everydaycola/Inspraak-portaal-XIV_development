using System.ComponentModel.DataAnnotations;
using UI_MVC.Models.Dto;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class AnswerOptionCrudViewModel
{
    [Key] public int Id { get; set; }
    public string AnswerOptionText { get; set; }
    public int Weight { get; set; }
    public List<AnswerOptionImpactDto> AnswerOptionImpacts { get; set; }
}