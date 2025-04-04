using System.ComponentModel.DataAnnotations;

namespace Domain.CitizenPanel;

public class PanelMember
{
    public Guid PanelMemberId { get; set; }
    public bool HasAnsweredAllQuestions { get; set; }
    [EmailAddress(ErrorMessage = "Panel member email is niet geldig.")]
    [MaxLength(321, ErrorMessage = "Panel member email is te lang.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Panel member moet deel zijn van een panel.")]
    public Panel Panel { get; set; }
    [Required(ErrorMessage = "Answer options are required.")]
    [MinLength(2, ErrorMessage = "Criteria vraag moet minimaal 2 opties hebben.")]
    [MaxLength(12, ErrorMessage = "Criteria vraag mag maximaal 12 opties hebben.")]
    public ICollection<CriteriaResponse> Responses { get; set; }
}