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
    // READ
    public Panel ReadPanel(Guid panelId);
    public Panel ReadPanelWithRepresentationGroup(Guid panelId);
    public Panel ReadPanelWithCriteriaAndAnswerOptions(Guid panelId);
    public Panel ReadPanelWithTimeLinesAndPostsAndSuggestionsAndVotesAndDocuments(Guid panelId);
    public IEnumerable<Panel> ReadAllPanels();
    public IEnumerable<Panel> ReadAllPanelsWithPostsAndSuggestions();
    public PanelMember ReadPanelMemberWithCriteriaResponses(Guid panelMemberId);
    public PanelMember ReadPanelMemberWithPanelAndCriteriaResponseAndCriteria(Guid panelMemberId);
    public IEnumerable<PanelMember> ReadAllPanelMembersForPanel(Guid panelId, bool onlyUnselected = false);
    public IEnumerable<PanelMember> ReadPanelMembersWithCriteriaAndResponsesByPanel(Guid panelId);
    public PlanningGroupMember ReadPlanningGroupMember(Guid planningsGroupMemberId);
    public PlanningGroupMember ReadPlanningsGroupMemberWithPanelAndIdentityUser(Guid planningsGroupMemberId);
    public IEnumerable<PlanningGroupMember> ReadAllPlanningGroupMembersWithIdentityUserByPanel(Guid panelId);
    public Post ReadPost(Guid postId);
    public Vote ReadVoteByPanelMemberAndSuggestionOrDefault(ApplicationUser user, Guid suggestionId);
    public Suggestion ReadSuggestion(Guid suggestionId);
    
    // CREATE
    public void CreatePanelMembers(ICollection<PanelMember> panelMembers);
    public void CreateTimeLine(Guid panelId, TimeLine timeLine);
    public void CreatePost<T>(Guid timeLineId, T post) where T : Post;
    public void CreatePlanningsGroupMember(PlanningGroupMember member);
    public void CreateVote(Vote vote);
    
    // UPDATE
    public void UpdatePanel(Panel panel);
    public void UpdatePanelMember(PanelMember panelMember);
    public void UpdatePanelMembers(ICollection<PanelMember> panelMembers);
    public void UpdateSuggestionPost(SuggestionPost suggestionPost);
    public void UpdateSuggestion(Suggestion suggestion);
    public void UpdatePost(Post post);
    public void UpdateVote(Vote vote);
    public void UpdatePlanningsGroupMember(PlanningGroupMember member);
    
    // DELETE
    public void DeletePanelMembers(ICollection<PanelMember> panelMembers);
    public void DeletePlanningGroupMember(PlanningGroupMember planningsGroupMember);
}