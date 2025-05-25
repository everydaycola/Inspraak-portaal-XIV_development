using System.Security.Claims;
using Domain;
using Domain.CitizenPanel;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Posts;
using Domain.Interfaces.Posts.PostItems;

namespace DAL.Interfaces;

public interface IPanelRepository
{
    //READ
    public Panel ReadPanel(Guid id);
    public IEnumerable<Panel> ReadAllPanels();
    public PanelMember ReadPanelMemberWithCriteriaResponses(Guid id);
    public PanelMember ReadPanelMemberWithPanel(Guid id);
    public Panel ReadPanelWithRepresentationGroup(Guid id);
    public IEnumerable<PanelMember> ReadPanelMembersWithCriteria(Guid id);
    public Panel ReadPanelWithCriteriaAndAnswerOptions(Guid panelId);
    public IEnumerable<PlanningGroupMember> ReadAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId);
    public Panel ReadPanelWithPostsAndSuggestionsAndVotes(Guid panelId);
    public PlanningGroupMember ReadPlanningGroupMember(Guid planningsGroupMemberId);
    public Post ReadPost(Guid id);
    public SuggestionPost ReadSuggestionPostSuggestionsAndWithVotes(Guid id);
    public Vote ReadVoteByPanelMemberAndSuggestionOrDefault(ApplicationUser user, Guid suggestionId);
    public Suggestion ReadSuggestion(Guid suggestionId);
    public IEnumerable<Panel> ReadAllPanelsWithPostsAndSuggestions();

    //UPDATE
    public void UpdatePanel(Panel panel);
    public void UpdatePanelMember(PanelMember member);

    public void UpdatePanelMembersToSelected(ICollection<PanelMember> selectedMembers);
    public void UpdateSuggestionPost(SuggestionPost suggestionPost);
    public void UpdateVote(Vote vote);
    public void UpdateSuggestion(Suggestion suggestion);
    //CREATE
    public void CreatePanelMember(PanelMember panelMember);
    public void CreatePanelMembers(List<PanelMember> panelMembers);
    public void CreatePost<T>(Guid panelId, T post) where T : Post;
    public void CreatePlanningsGroupMember(PlanningGroupMember member);
    public void CreateSummaryToMeetingPost(Guid meetingId, string uniqueFileName);
    public void CreateVote(Vote vote);
    
    //REMOVE
    public void RemoveAllUnselectedPanelmembers(Guid panelId);
    public void RemovePlanningGroupMember(Guid planningsGroupMemberId);
    public void RemoveVote(Vote vote);
}