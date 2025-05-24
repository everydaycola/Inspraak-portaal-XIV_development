using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels;

public class ParticipationViewModel
{
    [Microsoft.Build.Framework.Required]
    public string Name { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
}