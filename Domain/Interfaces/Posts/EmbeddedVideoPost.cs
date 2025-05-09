using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

public class EmbeddedVideoPost: Post
{
    [MaxLength(1000, ErrorMessage = "Video url is too long")]
    public string VideoUrl { get; set; }
}