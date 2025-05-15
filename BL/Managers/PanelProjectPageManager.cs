using System.ComponentModel.DataAnnotations;
using BL.Interfaces;
using DAL.Interfaces;
using Domain.CitizenPanel;
using Domain.Interfaces;
using Domain.Interfaces.Posts;
using Domain.Interfaces.Posts.PostItems;
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
    
    public Panel GetPanelWithPostsAndSuggestions(Guid panelId)
    {
        return _repo.ReadPanelWithPostsAndSuggestions(panelId);
    }
    
    public void AddSummaryToMeetingPost(Guid meetingId, string uniqueFileName)
    {
        _repo.AddSummaryToMeetingPost(meetingId, uniqueFileName);
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
    public void AddTextPost(Guid panelId, string title, string content, bool isVisibleForPanelMembers)
    {
        AddPost(panelId, new TextPost
        {
            Title = title,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = isVisibleForPanelMembers
        });
    }

    public void AddDocumentPost(Guid panelId, string title, string documentUrl, bool isVisibleForPanelMembers)
    {
        AddPost(panelId, new DocumentPost
        {
            Title = title,
            DocumentName = documentUrl,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = isVisibleForPanelMembers
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

    public void AddEmbedVideoPost(Guid panelId, string title, string videoUrl, bool visibleForPanelMember)
    {
        AddPost(panelId, new EmbeddedVideoPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            VideoUrl = videoUrl
        });
    }

    public void AddYoutubeVideoPost(Guid panelId, string title, string videoId, bool visibleForPanelMember)
    {
        AddPost(panelId, new YoutubeVideoPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            VideoId = videoId
        });
    }

    public void AddSuggestionPost(Guid panelId, string title, bool visibleForPanelMember)
    {
        AddPost(panelId, new SuggestionPost
        {
            Title = title,
            CreatedAt = DateTime.UtcNow,
            IsVisibleForPanelMembers = visibleForPanelMember,
            Suggestions = []
        });
    }

    public Suggestion AddSuggestionToPost(Guid PostId, string suggestionTitle, string owner)
    {
        var post = _repo.ReadPost(PostId);

        if (post is not SuggestionPost suggestionPost)
            // should not happen
            throw new InvalidCastException("Post is not a suggestion post");

        var suggestion = new Suggestion
        {
            Title = suggestionTitle,
            CreatedAt = DateTime.UtcNow,
            Owner = owner
        };
        suggestionPost.Suggestions.Add(suggestion);
        // update the post with the new suggestion
        _repo.UpdateSuggestionPost(suggestionPost);

        return suggestion;
    }
    
}