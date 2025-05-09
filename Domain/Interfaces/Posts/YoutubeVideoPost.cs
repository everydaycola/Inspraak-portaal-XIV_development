using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Posts;

public class YoutubeVideoPost : Post
{
    [MaxLength(100, ErrorMessage = "VideoID is too long")]
    public string VideoId { get; set; }
}