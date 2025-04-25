namespace Domain.Interfaces;

public abstract class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
}