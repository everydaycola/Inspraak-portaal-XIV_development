using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Posts.PostItems;

public class Suggestion
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Post moet een titel hebben")]
    [MaxLength(300, ErrorMessage = "naam is te lang")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Post moet een datum hebben")]
    public DateTime CreatedAt { get; set; }
    [MaxLength(300, ErrorMessage = "Email van suggestie eigenaar is te lang")]
    public string OwnerEmail { get; set; }
}