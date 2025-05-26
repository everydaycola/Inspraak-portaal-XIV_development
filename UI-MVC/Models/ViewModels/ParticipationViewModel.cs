using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models.ViewModels;

public class ParticipationViewModel
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
}