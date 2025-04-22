using System.ComponentModel.DataAnnotations;

namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid PanelMemberId { get; set; }
    public bool HasRegistered { get; set; }

    [EmailAddress(ErrorMessage = "Panel member email is niet geldig.")]
    [MaxLength(321, ErrorMessage = "Panel member email is te lang.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Panel member moet deel zijn van een panel.")]
    public Panel Panel { get; set; }
    
    public bool Selected { get; set; }

   
    [MaxLength(10, ErrorMessage = "Criteria antwoord mag maximaal 10 opties hebben.")]
    public ICollection<CriteriaResponse> Responses { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Fase moet een positief getal zijn.")]
    public int Phase { get; set; } = 1;
}