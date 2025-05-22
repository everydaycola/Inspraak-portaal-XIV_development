using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels.ExploreConceptViewModels;

public class QuestionWeightTipsViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Minimale score is verplicht.")]
    [Range(0, int.MaxValue, ErrorMessage = "Minimale score moet positief zijn.")]
    public int MinScore { get; set; }

    public int? MaxScore { get; set; }

    [Required(ErrorMessage = "De boodschap is verplicht.")]
    [StringLength(500, ErrorMessage = "De boodschap mag maximaal 500 tekens bevatten.")]
    [Display(Name = "Boodschap voor Gebruiker")]
    public string Message { get; set; }
}