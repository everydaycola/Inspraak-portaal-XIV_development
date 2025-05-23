namespace Domain.Interfaces.Posts;

public class PostGroup : Post
{
    public ICollection<Post> Posts { get; set; } = [];
}