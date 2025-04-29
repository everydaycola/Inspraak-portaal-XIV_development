namespace Domain.Interfaces.Posts;

public class MeetingPost : Post
{
    public ICollection<string> DocumentNames { get; set; }
}