using Domain.CitizenPanel;
using Domain.Interfaces.Posts.PostItems;

namespace Domain.Interfaces.Posts;

public class SuggestionPost : Post
{
    public ICollection<Suggestion> Suggestions { get; set; } = [];
    
}