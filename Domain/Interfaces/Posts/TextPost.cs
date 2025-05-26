using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Posts;

public class TextPost: Post
{
    [Required]
    [MaxLength(10000, ErrorMessage = "Post is te lang")]
    public string Content { get; set; }
}