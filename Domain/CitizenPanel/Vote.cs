using Domain.Enums;
using Domain.Interfaces.Posts.PostItems;

namespace Domain.CitizenPanel;

public class Vote
{
    public Suggestion Suggestion { get; set; }
    public ApplicationUser Owner { get; set; }
    public VoteType VoteType { get; set; }
}