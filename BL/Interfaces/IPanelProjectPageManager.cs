using System.Security.Claims;
using Domain;
using Domain.CitizenPanel;
using Domain.Enums;
using Domain.Interfaces.Posts.PostItems;

namespace BL.Interfaces;

public interface IPanelProjectPageManager
{
    // GET
    public Panel GetPanelWithPostsAndSuggestionsAndVotes(Guid panelId);
    public Suggestion GetSuggestion(Guid suggestionId);

    // CHANGE
    public void ChangeVote(ApplicationUser user, Guid suggestionId, VoteType voteType);
    public void ChangeSuggestionVisibility(Guid suggesionId);
    // ADD
    public void AddTextPost(Guid panelId,string title, string content, bool isVisibleForPanelMembers, bool isGloballyVisible);
    public void AddDocumentPost(Guid panelId, string title, string documentUrl, bool isVisibleForPanelMembers, bool isGloballyVisible);
    public void AddMeetingPost(Guid panelId, string title, DateTime meetingDateTime, bool visibleForPanelMember);
    public void AddEmbedVideoPost(Guid panelId, string title, string videoUrl, bool visibleForPanelMember, bool isGloballyVisible);
    public void AddYoutubeVideoPost(Guid panelId, string title, string videoId, bool visibleForPanelMember, bool isGloballyVisible);
    public void AddSuggestionPost(Guid panelId, string title, bool visibleForPanelMember);
    public void AddSummaryToMeetingPost(Guid meetingId, string uniqueFileName);
    public void AddSuggestionToPost(Guid PostId, string suggestion, string owner);

    public void AddGoogleFormLink(Guid panelId, string title, string embeddedIframeLink, bool visibleForPanelMember,
        bool isGloballyVisible);
    // REMOVE
}