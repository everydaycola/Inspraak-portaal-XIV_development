using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BL.Interfaces;
using DAL.Interfaces;
using Domain;
using Domain.CitizenPanel;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Posts;
using Domain.Interfaces.Posts.PostItems;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BL.Managers;

public class PanelProjectPageManager : IPanelProjectPageManager
{
    private readonly ILogger<PanelManager> _logger;
    private readonly IPanelRepository _repo;

    public PanelProjectPageManager(ILogger<PanelManager> logger, IPanelRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }
    
    public Panel GetPanelWithTimeLinesAndPostsAndSuggestionsAndVotesAndDocuments(Guid panelId)
    {
        return _repo.ReadPanelWithTimeLinesAndPostsAndSuggestionsAndVotesAndDocuments(panelId);
    }

    public Suggestion GetSuggestion(Guid suggestionId)
    {
        return _repo.ReadSuggestion(suggestionId);
    }

    public Post GetPost(Guid postId)
    {
        return _repo.ReadPost(postId);
    }

    public SuggestionPost GetSuggestionPostSuggestionsAndWithVotes(Guid suggestionPostId)
    {
        return _repo.ReadSuggestionPostSuggestionsAndWithVotes(suggestionPostId);
    }

    public void ChangeVote(ApplicationUser user, Guid suggestionId, VoteType voteType)
    {
        var vote = _repo.ReadVoteByPanelMemberAndSuggestionOrDefault(user, suggestionId);
        if (vote is null)
        {
            _repo.CreateVote(new Vote
            {
                Owner = user,
                Suggestion = _repo.ReadSuggestion(suggestionId),
                VoteType = voteType
            });
        }
        else
        {
            vote.VoteType = voteType;
            _repo.UpdateVote(vote);
        }
    }
    
    public void ChangeSuggestionVisibility(Guid suggestionId)
    {
        var suggestion = _repo.ReadSuggestion(suggestionId);
        suggestion.IsGloballyVisible = !suggestion.IsGloballyVisible;
        _repo.UpdateSuggestion(suggestion);
    }

    public void ChangeExecutedToggle(Guid suggestionId)
    {
        var suggestion = _repo.ReadSuggestion(suggestionId);
        suggestion.IsExecuted = !suggestion.IsExecuted;
        _repo.UpdateSuggestion(suggestion);
    }


    public void ChangeSuggestionPostVotingStatus(Guid postId)
    {
        var suggestionPost = (SuggestionPost)_repo.ReadPost(postId);
        suggestionPost.IsVotingOpen = !suggestionPost.IsVotingOpen;
        _repo.UpdateSuggestionPost(suggestionPost);
    }

    public void AddSummaryToMeetingPost(Guid meetingId, string uniqueFileName)
    {
        if (_repo.ReadPost(meetingId) is not MeetingPost meetingPost)
        {
            _logger.Log(LogLevel.Critical, "Meeting post with id " + meetingId + " does not exist.");
            return;
        }
        meetingPost.DocumentNames.Add(uniqueFileName);
        _repo.UpdatePost(meetingPost);
    }
    
    public void AddDocumentToPost(Guid meetingId, string uniqueFileName)
    {
        var post = _repo.ReadPost(meetingId);
        
        switch (post)
        {
            // Add the document to the appropriate post type
            case MeetingPost mp:
                mp.DocumentNames.Add(uniqueFileName);
                _repo.UpdatePost(mp);
                break;
            case SuggestionPost sp:
                sp.DocumentNames.Add(uniqueFileName);
                _repo.UpdatePost(sp);
                break;
            default:
                throw new InvalidOperationException($"Post type {post.GetType().Name} does not support document attachments");
        }
    }
    
    // Generic helper method for post validation and creation
    private void AddPost<T>(Guid timeLineId, T post) where T : Post
    {
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(post, new ValidationContext(post), validationResults, true))
        {
            throw new ValidationException(string.Join("\n", validationResults.Select(x => x.ErrorMessage)));
        }

        _repo.CreatePost(timeLineId, post);
    }

// Simplified post methods
    public void AddTextPost(Guid timeLineId, string title, string content, bool isVisibleForPanelMembers, bool isGloballyVisible)
    {
        AddPost(timeLineId, new TextPost
        {
            Title = title,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = isVisibleForPanelMembers,
            IsGloballyVisible = isGloballyVisible,
        });
    }

    public void AddDocumentPost(Guid timeLineId, string title, string documentUrl, bool isVisibleForPanelMembers, bool isGloballyVisible)
    {
        AddPost(timeLineId, new DocumentPost
        {
            Title = title,
            DocumentName = documentUrl,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = isVisibleForPanelMembers,
            IsGloballyVisible = isGloballyVisible,
        });
    }

    public void AddMeetingPost(Guid timeLineId, string title, DateTime meetingDateTime, bool isVisibleForPanelMembers)
    {
        AddPost(timeLineId, new MeetingPost
        {
            Title = title,
            CreatedAt = meetingDateTime,
            IsVisibleForPanelMembers = isVisibleForPanelMembers,
        });
    }

    public void AddEmbedVideoPost(Guid timeLineId, string title, string videoUrl, bool visibleForPanelMember, bool isGloballyVisible)
    {
        AddPost(timeLineId, new EmbeddedVideoPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            VideoUrl = videoUrl,
            IsGloballyVisible = isGloballyVisible,
        });
    }

    public void AddYoutubeVideoPost(Guid timeLineId, string title, string videoId, bool visibleForPanelMember, bool isGloballyVisible)
    {
        AddPost(timeLineId, new YoutubeVideoPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            VideoId = videoId,
            IsGloballyVisible = isGloballyVisible
        });
    }

    public void AddSuggestionPost(Guid timeLineId, string title, bool visibleForPanelMember,bool isNeutralVoteAllowed, bool isVotingOpen, double votingMajorityFactor )
    {
        AddPost(timeLineId, new SuggestionPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            IsNeutralVoteAllowed = isNeutralVoteAllowed,
            Suggestions = [],
            IsVotingOpen = isVotingOpen,
            VotingMajorityFactor = votingMajorityFactor
        });
    }
    
    public void AddTimeLine(Guid panelId, string title, DateTime timeLineTime)
    {
        var timeLine = new TimeLine
        {
            Title = title,
            CreatedAt = timeLineTime,
        };
        
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(timeLine, new ValidationContext(timeLine), validationResults, true))
        {
            throw new ValidationException(string.Join("\n", validationResults.Select(x => x.ErrorMessage)));
        }
        
        _repo.CreateTimeLine(panelId, timeLine);
    }

    public void AddSuggestionToPost(Guid PostId, string suggestionTitle, string owner)
    {
        if (_repo.ReadPost(PostId) is not SuggestionPost suggestionPost)
            // should not happen
            throw new InvalidCastException("Post is not a suggestion post");

        suggestionPost.Suggestions.Add(new Suggestion
        {
            Title = suggestionTitle,
            CreatedAt = DateTime.UtcNow,
            OwnerEmail = owner
        });
        // update the post with the new suggestion
        _repo.UpdateSuggestionPost(suggestionPost);
    }

    public void AddGoogleFormLink(Guid timeLineId, string title, string embeddedIframeLink, bool visibleForPanelMember, bool isGloballyVisible)
    {
        AddPost(timeLineId, new EmbeddedGoogleFormLink()
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            EmbeddedIframeUrl = embeddedIframeLink,
            IsGloballyVisible = isGloballyVisible
        });
    }
}