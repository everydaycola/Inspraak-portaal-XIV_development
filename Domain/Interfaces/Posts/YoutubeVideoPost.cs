using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces.Posts;

public class YoutubeVideoPost : Post
{
    // youtube video id of the video
    // ex. PupZqWhC5o0
    [MaxLength(100, ErrorMessage = "VideoID is too long")]
    public string VideoId { get; set; }
}