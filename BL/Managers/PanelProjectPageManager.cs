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

    public PanelProjectPageManager(ILogger<PanelManager> logger, IPanelRepository repo, IUserRepository userRepo, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _repo = repo;
    }
    
    public Panel GetPanelWithPostsAndSuggestionsAndVotes(Guid panelId)
    {
        return _repo.ReadPanelWithPostsAndSuggestionsAndVotes(panelId);
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
    
    // Generic helper method for post validation and creation
    private void AddPost<T>(Guid panelId, T post) where T : Post
    {
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(post, new ValidationContext(post), validationResults, true))
        {
            throw new ValidationException(string.Join("\n", validationResults.Select(x => x.ErrorMessage)));
        }

        _repo.CreatePost(panelId, post);
    }

// Simplified post methods
    public void AddTextPost(Guid panelId, string title, string content, bool isVisibleForPanelMembers, bool isGloballyVisible)
    {
        AddPost(panelId, new TextPost
        {
            Title = title,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = isVisibleForPanelMembers,
            IsGloballyVisible = isGloballyVisible
        });
    }

    public void AddDocumentPost(Guid panelId, string title, string documentUrl, bool isVisibleForPanelMembers, bool isGloballyVisible)
    {
        AddPost(panelId, new DocumentPost
        {
            Title = title,
            DocumentName = documentUrl,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = isVisibleForPanelMembers,
            IsGloballyVisible = isGloballyVisible
        });
    }

    public void AddMeetingPost(Guid panelId, string title, DateTime meetingDateTime, bool isVisibleForPanelMembers)
    {
        AddPost(panelId, new MeetingPost
        {
            Title = title,
            DocumentNames = new List<string>(),
            CreatedAt = meetingDateTime,
            IsVisibleForPanelMembers = isVisibleForPanelMembers
        });
    }

    public void AddEmbedVideoPost(Guid panelId, string title, string videoUrl, bool visibleForPanelMember, bool isGloballyVisible)
    {
        AddPost(panelId, new EmbeddedVideoPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            VideoUrl = videoUrl,
            IsGloballyVisible = isGloballyVisible
        });
    }

    public void AddYoutubeVideoPost(Guid panelId, string title, string videoId, bool visibleForPanelMember, bool isGloballyVisible)
    {
        AddPost(panelId, new YoutubeVideoPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            VideoId = videoId,
            IsGloballyVisible = isGloballyVisible
        });
    }

    public void AddSuggestionPost(Guid panelId, string title, bool visibleForPanelMember, bool isVotingOpen)
    {
        AddPost(panelId, new SuggestionPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            Suggestions = [],
            IsVotingOpen = isVotingOpen
        });
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
}