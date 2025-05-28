using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Posts;

namespace Domain.CitizenPanel;

public class TimeLine
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Post moet een titel hebben")]
    [MaxLength(300, ErrorMessage = "naam is te lang")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Post moet een datum hebben")]
    public DateTime CreatedAt { get; set; }
    public ICollection<Post> Posts { get; set; } = [];
}