using System.ComponentModel.DataAnnotations;
using Domain.CitizenPanel;

namespace Domain.Interfaces.Posts.PostItems;

public class Suggestion
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Post moet een titel hebben")]
    [MaxLength(300, ErrorMessage = "naam is te lang")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Post moet een datum hebben")]
    public DateTime CreatedAt { get; set; }
    // not used because there is no real reason (yet)
    // [EmailAddress(ErrorMessage = "It seems this is not a correct email address")]
    [MaxLength(300, ErrorMessage = "Email van aanbeveling eigenaar is te lang")]
    public string OwnerEmail { get; set; }
    public ICollection<Vote> Votes { get; set; } = [];
}