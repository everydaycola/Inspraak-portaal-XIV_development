using System.Security.Claims;
using DAL.EF;
using DAL.Interfaces;
using Domain;
using Domain.CitizenPanel;
using Domain.Interfaces;
using Domain.Interfaces.Posts;
using Domain.Interfaces.Posts.PostItems;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PanelRepository : IPanelRepository
{
    private readonly CitizenPanelDbContext _context;

    public PanelRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }

    // READ
    public Panel ReadPanel(Guid panelId)
    {
        return _context.Panels.Find(panelId);
    }
    
    public Panel ReadPanelWithRepresentationGroup(Guid panelId)
    {
        return _context.Panels
            .Include(p => p.RepresentationGroup)
            .Single(p => p.Id == panelId);
    }
    
    public Panel ReadPanelWithCriteriaAndAnswerOptions(Guid panelId)
    {
        return _context.Panels
            .Include(p => p.Criteria)
            .ThenInclude(p => p.AnswerOptions)
            .Single(p => p.Id == panelId);
    }
    
    public Panel ReadPanelWithTimeLines(Guid panelId)
    {
        return _context.Panels
            .Include(p => p.Timelines)
            .Single(p => p.Id == panelId);
    }
    
    public Panel ReadPanelWithTimeLinesAndPostsAndSuggestionsAndVotesAndDocuments(Guid panelId)
    {
        return _context.Panels
            .Include(p => p.Timelines)
            .ThenInclude(t => t.Posts)
            .ThenInclude(p => (p as SuggestionPost).Suggestions)
            .ThenInclude(s => s.Votes)
            .ThenInclude(v => v.Owner)
            .Single(p => p.Id == panelId);
    }

    public IEnumerable<Panel> ReadAllPanels()
    {
        return _context.Panels.ToList();
    }
    
    public IEnumerable<Panel> ReadAllPanelsWithPostsAndSuggestions()
    {
        return _context.Panels
            .Include(p => p.Timelines)
            .ThenInclude(t => t.Posts)
            .ThenInclude(post => (post as SuggestionPost).Suggestions)
            .ToList();
    }
    
    public PanelMember ReadPanelMemberWithCriteriaResponses(Guid panelMemberId)
    {
        return _context.PanelMembers
            .Include(pm => pm.Responses)
            .Single(p => p.PanelMemberId == panelMemberId);
    }

    public PanelMember ReadPanelMemberWithPanelAndCriteriaResponseAndCriteria(Guid panelMemberId)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .Include(pm => pm.Responses)
            .ThenInclude(pm => pm.Criteria)
            .Single(p => p.PanelMemberId == panelMemberId);
    }

    public IEnumerable<PanelMember> ReadPanelMembersWithCriteriaAndResponsesByPanel(Guid panelId)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .ThenInclude(p => p.Criteria)
            .ThenInclude(c => c.AnswerOptions)
            .Include(pm => pm.Responses)
            .Where(pm => pm.Panel.Id == panelId)
            .ToList();
    }
    
    public IEnumerable<PanelMember> ReadAllPanelMembersForPanel(Guid panelId, bool onlyUnselected = false)
    {
        return _context.PanelMembers.Include(pm => pm.Panel)
            .Where(pm => pm.Panel.Id == panelId)
            .Where(pm => onlyUnselected && !pm.Selected)
            .ToList();
    }
    
    public PlanningGroupMember ReadPlanningGroupMember(Guid planningsGroupMemberId)
    {
        return _context.PlanningGroupMembers.Find(planningsGroupMemberId);
    }

    public PlanningGroupMember ReadPlanningsGroupMemberWithPanelAndIdentityUser(Guid planningsGroupMemberId)
    {
        return _context.PlanningGroupMembers
            .Include(p => p.User)
            .Include(p => p.Panel)
            .FirstOrDefault(p => p.Id == planningsGroupMemberId);
    }

    public IEnumerable<PlanningGroupMember> ReadAllPlanningGroupMembersWithIdentityUserByPanel(Guid panelId)
    {
        return _context.PlanningGroupMembers
            .Include(pgm => pgm.User)
            .Where(pgm => pgm.Panel.Id == panelId)
            .ToList();
    }
    
    public Post ReadPost(Guid postId)
    {
        return _context.Posts.Find(postId);
    }

    public SuggestionPost ReadSuggestionPostSuggestionsAndWithVotes(Guid id)
    {
        return _context.Posts
            .OfType<SuggestionPost>()
            .Include(sp => sp.Suggestions)
            .ThenInclude(s => s.Votes)
            .Single(sp => sp.Id == id);
    }
    
    public Vote ReadVoteByPanelMemberAndSuggestionOrDefault(ApplicationUser user, Guid suggestionId)
    {
        return _context.Votes
            .Include(v => v.Owner)
            .Include(v => v.Suggestion)
            .SingleOrDefault(v => 
                v.Owner == user && 
                v.Suggestion.Id == suggestionId);

    }

    public Suggestion ReadSuggestion(Guid suggestionId)
    {
        return _context.Suggestions.Find(suggestionId);
    }
    
    public TimeLine ReadTimelineWithPosts(Guid timeLineId)
    {
        return _context.TimeLines.Find(timeLineId);
    }
    
    
    // CREATE
    public void CreatePanelMembers(ICollection<PanelMember> panelMembers)
    {
        foreach (var panelMember in panelMembers)
        {
            _context.PanelMembers.Add(panelMember);
        }
        _context.SaveChanges();
    }
    
    public void CreateTimeLine(Guid panelId,TimeLine timeLine)
    {
        var panel = ReadPanelWithTimeLines(panelId);
        if (panel == null) return;
        panel.Timelines.Add(timeLine);
        _context.TimeLines.Add(timeLine);
        _context.SaveChanges();
    }
    
    public void CreatePost<T>(Guid timeLineId, T post) where T : Post
    {
        var timeLine = ReadTimelineWithPosts(timeLineId);
        if (timeLine == null) return;
        timeLine.Posts.Add(post);
        _context.TimeLines.Update(timeLine);
        _context.SaveChanges();
    }
    
    public void CreatePlanningsGroupMember(PlanningGroupMember member)
    {
        _context.PlanningGroupMembers.Add(member);
        _context.SaveChanges();
    }

    public void CreateVote(Vote vote)
    {
        _context.Votes.Add(vote);
        _context.SaveChanges();
    }
    
    
    // UPDATE
    public void UpdatePanel(Panel panel)
    {
        _context.Panels.Update(panel);
        _context.SaveChanges();
    }
    
    public void UpdatePanelMember(PanelMember panelMember)
    {
        _context.PanelMembers.Update(panelMember);
        _context.SaveChanges();
    }
    
    public void UpdatePanelMembers(ICollection<PanelMember> panelMembers)
    {
        foreach (var panelMember in panelMembers)
        {
            _context.PanelMembers.Update(panelMember);
        }
        _context.SaveChanges();
    }
    
    public void UpdateSuggestionPost(SuggestionPost suggestionPost)
    {
        _context.Posts.Update(suggestionPost);
        _context.SaveChanges();
    }
    
    public void UpdateSuggestion(Suggestion suggestion)
    {
        _context.Suggestions.Update(suggestion);
        _context.SaveChanges();
    }
    
    public void UpdatePost(Post post)
    {
        _context.Posts.Update(post);
        _context.SaveChanges();
    }

    public void UpdateVote(Vote vote)
    {
        _context.Votes.Update(vote);
        _context.SaveChanges();
    }

    public void UpdatePlanningsGroupMember(PlanningGroupMember member)
    {
        _context.PlanningGroupMembers.Update(member);
        _context.SaveChanges();
    }


    // DELETE
    public void DeletePanelMembers(ICollection<PanelMember> panelMembers)
    {
        foreach (var panelMember in panelMembers)
        {
            _context.PanelMembers.Remove(panelMember);
        }
        _context.SaveChanges();
    }
    
    public void DeletePlanningGroupMember(PlanningGroupMember planningsGroupMember)
    {
        _context.PlanningGroupMembers.Remove(planningsGroupMember);
        _context.SaveChanges();
    }
    
   
}