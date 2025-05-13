using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

public abstract class Post
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Post moet een titel hebben")]
    [MaxLength(300, ErrorMessage = "Name is too long")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Post moet een datum hebben")]
    public DateTime CreatedAt { get; set; }
    [Required(ErrorMessage = "Post moet een zichbaarheids status hebben")]
    public bool IsVisibleForPanelMembers { get; set; } = false;
}