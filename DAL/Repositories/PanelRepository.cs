using DAL.EF;
using DAL.Interfaces;
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

    public Panel ReadPanel(Guid id)
    {
        return _context.Panels.Find(id);
    }

    public IEnumerable<Panel> ReadAllPanels()
    {
        return _context.Panels.ToList();
    }


    public PanelMember ReadPanelMemberWithCriteriaResponses(Guid id)
    {
        return _context.PanelMembers
            .Include(pm => pm.Responses)
            .Single(p => p.PanelMemberId == id);
    }

    public PanelMember ReadPanelMemberWithPanel(Guid id)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .Include(pm => pm.Responses)
            .ThenInclude(pm => pm.Criteria)
            .Single(p => p.PanelMemberId == id);
    }

    public Post ReadPost(Guid id)
    {
        return _context.Posts.Find(id);
    }

    public IEnumerable<Panel> ReadAllPanelsWithPosts()
    {
        return _context.Panels
            .Include(p => p.Posts);
    }


    public void CreatePlanningsGroupMember(PlanningGroupMember member)
    {
        _context.PlanningGroupMembers.Add(member);
        _context.SaveChanges();
    }

    public void RemoveAllUnselectedPanelmembers(Guid panelId)
    {
        var unselectedMembers = _context.PanelMembers
            .Include(pm => pm.Panel)
            .Where(pm => pm.Panel.Id == panelId && !pm.Selected && pm.HasRegistered)
            .ToList();

        if (!unselectedMembers.Any()) return;
        _context.PanelMembers.RemoveRange(unselectedMembers);
        _context.SaveChanges();
    }

    public void RemovePlanningGroupMember(Guid planningsGroupMemberId)
    {
        var member = _context.PlanningGroupMembers.Find(planningsGroupMemberId);
        _context.PlanningGroupMembers.Remove(member);
        _context.SaveChanges();
    }

    public void AddSummaryToMeetingPost(Guid meetingId, string uniqueFileName)
    {
        MeetingPost meetingPost = _context.Posts.Find(meetingId) as MeetingPost;
        if (meetingPost == null)
        {
            throw new Exception("Meeting post not found");
        }
        meetingPost.DocumentNames.Add(uniqueFileName);
        _context.Posts.Update(meetingPost);
        _context.SaveChanges();
    }

    // public Panel ReadPanelWithCriteriaAndAnsweroptions(Guid id)
    // {
    //     return _context.Panels
    //         .Include(p => p.Criteria)
    //         .ThenInclude(p => p.AnswerOptions)
    //         .Single(p => p.Id == id);
    // }

    public Panel ReadPanelWithRepresentationGroup(Guid id)
    {
        return _context.Panels
            .Include(p => p.RepresentationGroup)
            .Single(p => p.Id == id);
    }

    public IEnumerable<PanelMember> ReadPanelMembersWithCriteria(Guid id)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .ThenInclude(p => p.Criteria)
            .ThenInclude(c => c.AnswerOptions)
            .Include(pm => pm.Responses)
            .Where(pm => pm.Panel.Id == id)
            .ToList();
    }

    public IEnumerable<PanelMember> ReadPanelMembersWithResponses(Guid id)
    {
        return _context.PanelMembers
            .Include(pm => pm.Panel)
            .Include(pm => pm.Responses)
            .Where(pm => pm.Panel.Id == id)
            .ToList();
    }

    public void UpdateSuggestionPost(SuggestionPost suggestionPost)
    {
        _context.Posts.Update(suggestionPost);
        _context.SaveChanges();
    }

    public void UpdatePanelMember(PanelMember member)
    {
        _context.PanelMembers.Update(member);
        _context.SaveChanges();
    }

    public void UpdatePanelMembersToSelected(ICollection<PanelMember> selectedMembers)
    {
        foreach (var member in selectedMembers)
        {
            member.Selected = true;
            _context.PanelMembers.Update(member);
        }

        _context.SaveChanges();
    }

    public void CreatePanelMember(PanelMember panelMember)
    {
        _context.PanelMembers.Add(panelMember);
        _context.SaveChanges();
    }

    public void CreatePanelMembers(List<PanelMember> panelMembers)
    {
        panelMembers.ForEach(pm => _context.PanelMembers.Add(pm));
        _context.SaveChanges();
    }

    public IEnumerable<PlanningGroupMember> ReadAllPlanningGroupMembersWithIdentityUserForPanel(Guid panelId)
    {
        return _context.PlanningGroupMembers
            .Include(pgm => pgm.User)
            .Where(pgm => pgm.Panel.Id == panelId)
            .ToList();
    }
    
    // Generic method to handle any type of post
    public void CreatePost<T>(Guid panelId, T post) where T : Post
    {
        _context.Posts.Add(post);
        var panel = ReadPanelWithPostsAndSuggestions(panelId);
        if (panel == null) return;
        panel.Posts.Add(post);
        _context.SaveChanges();
    }

    public ICollection<PanelMember> ReadPanelMembersAndRepresentationGroup(Guid id)
    {
        return _context.PanelMembers.Include(pm => pm.Panel)
            .ThenInclude(pm => pm.RepresentationGroup)
            .Where(pm => pm.Panel.Id == id)
            .ToList();
    }

    public Panel ReadPanelWithCriteriaAndAnswerOptions(Guid panelId)
    {
        return _context.Panels
            .Include(p => p.Criteria)
            .ThenInclude(p => p.AnswerOptions)
            .Single(p => p.Id == panelId);
    }

    public Panel ReadPanelWithPostsAndSuggestions(Guid panelId)
    {
        return _context.Panels
            .Include(p => p.Posts)
            .ThenInclude(p => (p as SuggestionPost).Suggestions)
            .Single(p => p.Id == panelId);
    }

    public PlanningGroupMember ReadPlanningGroupMember(Guid planningsGroupMemberId)
    {
        return _context.PlanningGroupMembers.Find(planningsGroupMemberId);
    }

    public void UpdatePanel(Panel panel)
    {
        _context.Panels.Update(panel);
        _context.SaveChanges();
    }
}