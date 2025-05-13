namespace Domain.Interfaces.Posts;

public class SuggestionPost : Post
{
    public ICollection<string> Suggestion { get; set; }
}