using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Posts;

public abstract class Post
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Post moet een titel hebben")]
    [MaxLength(300, ErrorMessage = "naam is te lang")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Post moet een datum hebben")]
    public DateTime CreatedAt { get; set; }
    [Required(ErrorMessage = "Post moet een zichbaarheids status hebben")]
    public bool IsVisibleForPanelMembers { get; set; } = false;
    public bool IsGloballyVisible { get; set; } = false;
}