using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

public class EmbeddedVideoPost: Post
{
    // direct link to the video
    // like https://packaged-media.redd.it/3fzkdhowttye1/pb/m2-res_1920p.mp4?m=DASHPlaylist.mpd&v=1&e=1746990000&s=4863758cd3af10565d451effb540c3a15dcb9c5f
    [MaxLength(1000, ErrorMessage = "Video url is too long")]
    public string VideoUrl { get; set; }
}