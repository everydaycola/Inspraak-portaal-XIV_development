using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Posts;

public class EmbeddedGoogleFormLink : Post
{
    [MaxLength(1000, ErrorMessage = "Video url is too long")]
    public string EmbeddedIframeUrl { get; set; }
}