using System.Collections;
using BL.Interfaces;
using DAL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.Extensions.Logging;

namespace BL.Managers;

public class CriteriaManager : ICriteriaManager
{
    private readonly ILogger<CriteriaManager> _logger;
    private readonly ICriteriaRepository _repo;
    private readonly IPanelManager _panelManager;

    public CriteriaManager(ILogger<CriteriaManager> logger, ICriteriaRepository repo, IPanelManager panelManager)
    {
        _repo = repo;
        _logger = logger;
        _panelManager = panelManager;
    }
    public Panel GetAllCriteriaWithValuesForPanel(Guid panelId)
    {
        return _repo.ReadAllCriteriaWithValuesForPanel(panelId);
    }

    /*public CriteriaValue GetCriteriaValueBasedOnCriteriaAndValue(Guid CriteriaId, string criteriaValue)
    {
        return _repo.ReadCriteriaValueBasedOnCriteriaAndValue(CriteriaId, criteriaValue);
    }*/

    public IEnumerable<Criteria> GetAllNonDefaultCriteriaWithValuesForPanel(Guid panelId)
    {
        return _repo.ReadAllNonDefaultCriteriaWithValuesForPanel(panelId);
    }
    
    public Dictionary<string, ICollection<PanelMember>> GetPanelMembersGroupedByResponses(Guid panelId)
    {
        var result = new Dictionary<string, ICollection<PanelMember>>();
        var panelMembers = _panelManager.GetAllPanelMembersForPanel(panelId);
        foreach (var member in panelMembers)
        {
            var groupName = string.Join("-", member.Responses.OrderBy(r => r.Criteria.Name).Select(r => r.SelectedOption).ToList());
            if (!result.TryGetValue(groupName, out var value))
            {
                value = new List<PanelMember>();
                result[groupName] = value; 
            }

            value.Add(member);
        }
        return result;
    }
    
    public Dictionary<string, ICollection<PanelMember>> GetPanelMembersWhichCompletedExtraCriteriaGroupedByResponses(Guid panelId)
    {
        var result = new Dictionary<string, ICollection<PanelMember>>();
        var panelMembers = _panelManager.GetAllPanelMembersWhichAnsweredAllQuestionsWithCriteria(panelId);
        foreach (var member in panelMembers)
        {
            var groupName = string.Join("-", member.Responses.OrderBy(r => r.Criteria.Name).Select(r => r.SelectedOption).ToList());
            if (!result.TryGetValue(groupName, out var value))
            {
                value = new List<PanelMember>();
                result[groupName] = value; 
            }

            value.Add(member);
        }
        return result;
    }

    public Dictionary<string, ICollection<PanelMember>> GetPanelMembersGroupedByResponsesForDefaultCriteria(Guid panelId)
    {
        var result = new Dictionary<string, ICollection<PanelMember>>();
        var panelMembers = _panelManager.GetAllPanelMembersForPanel(panelId);
        foreach (var member in panelMembers)
        {
            var groupName = string.Join("-", member.Responses.Where(r => r.Criteria.IsDefault).Select(r => r.SelectedOption).ToList());
            if (!result.TryGetValue(groupName, out var value))
            {
                value = new List<PanelMember>();
                result[groupName] = value; 
            }

            value.Add(member);
        }
        return result;
    }

    public void SavePanelMemberCriteriaResponses(Guid panelId,Dictionary<string, string> CriteriaAnswers, PanelMember member)
    {
        foreach (var entry in CriteriaAnswers)
        {
            string criteriaName = entry.Key;
            string selectedOption = entry.Value;
            var criteria = GetCriteriaByNameWithAnswerOptions(panelId, criteriaName);
            if (criteria != null)
            {
                var validOptions = criteria.AnswerOptions;
                if (validOptions.Any(option => option.Option == selectedOption))
                {
                    member.Responses.Add(new CriteriaResponse
                    {
                        Criteria = criteria,
                        SelectedOption = selectedOption,
                    });
                    member.HasAnsweredAllQuestions = true;
                    _panelManager.UpdatePanelMember(member);
                }
                else
                {
                    _logger.Log(LogLevel.Critical, "Member " + member.PanelMemberId + " tried inserting an invalid option for a criteria question.");
                }
            }
            _logger.Log(LogLevel.Critical, "Member " + member.PanelMemberId + " tried submitting a non existing criteria.");
        }
    }

    /*public IEnumerable<CriteriaGroup> GetAllCriteriaGroupForPanel(Guid panelId)
    {
        return _repo.ReadAllCriteriaGroupForPanel(panelId);
    }*/
    
    /*public CriteriaGroup GetCriteriaGroupByMemberId(Guid memberId)
    {
        return _repo.ReadCriteriaGroupByMemberId(memberId);
    }*/

    /*public CriteriaGroup GetCriteriaGroupById(Guid criteriaGroupId)
    {
        return _repo.ReadCriteriaGroupByid(criteriaGroupId);
    }*/
    
    public Criteria GetCriteriaByName(Guid panelId,string critName)
    {
        return _repo.ReadCriteriaByName(panelId, critName);
    }
    public Criteria GetCriteriaByNameWithAnswerOptions(Guid panelId,string critName)
    {
        return _repo.ReadCriteriaByNameWithAnswerOptions(panelId, critName);
    }
}
