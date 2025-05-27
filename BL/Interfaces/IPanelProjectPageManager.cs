using System.Security.Claims;
using Domain;
using Domain.CitizenPanel;
using Domain.Enums;
using Domain.Interfaces.Posts.PostItems;

namespace BL.Interfaces;

public interface IPanelProjectPageManager
{
    // GET
    public Panel GetPanelWithTimeLinesAndPostsAndSuggestionsAndVotesAndDocuments(Guid panelId);
    public Suggestion GetSuggestion(Guid suggestionId);

    // CHANGE
    public void ChangeVote(ApplicationUser user, Guid suggestionId, VoteType voteType);
    public void ChangeSuggestionVisibility(Guid suggesionId);
    public void ChangeExecutedToggle(Guid suggestionId);
    
    // ADD
    public void AddTimeLine(Guid panelId, string title, DateTime timeLineTime);
    public void AddTextPost(Guid timeLineId,string title, string content, bool isVisibleForPanelMembers, bool isGloballyVisible );
    public void AddDocumentPost(Guid timeLineId, string title, string documentUrl, bool isVisibleForPanelMembers, bool isGloballyVisible );
    public void AddMeetingPost(Guid timeLineId, string title, DateTime meetingDateTime, bool visibleForPanelMember );
    public void AddEmbedVideoPost(Guid timeLineId, string title, string videoUrl, bool visibleForPanelMember, bool isGloballyVisible );
    public void AddYoutubeVideoPost(Guid timeLineId, string title, string videoId, bool visibleForPanelMember, bool isGloballyVisible );
    public void AddSuggestionPost(Guid timeLineId, string title, bool visibleForPanelMember );
    public void AddGoogleFormLink(Guid timeLineId, string title, string embeddedIframeLink, bool visibleForPanelMember, bool isGloballyVisible);
    public void AddSuggestionToPost(Guid postId, string suggestion, string owner);
    public void AddDocumentToPost(Guid meetingId, string uniqueFileName);

   
    // REMOVE
}