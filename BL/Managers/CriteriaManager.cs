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
        Dictionary<string, ICollection<PanelMember>> result = new();
        var panelMembers = _panelManager.GetAllPanelMembersForPanel(panelId);
        foreach (var member in panelMembers)
        {
            var groupName = string.Join("-", member.Responses.Select(r => r.SelectedOption));
            if (!result.ContainsKey(groupName))
            {
                result[groupName] = new List<PanelMember>(); 
            }
            result[groupName].Add(member);
        }
        return result;
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

    /*public CriteriaGroup AssignMemberToCriteriaGroup(Guid panelId, Dictionary<string, string> CriteriaAnswers, PanelMember member)
    {
        string groupName = string.Join("-", CriteriaAnswers.Values);

        ICollection<CriteriaAnswer> criteriaAnswers = new List<CriteriaAnswer>();
        foreach (var crit in CriteriaAnswers)
        {
            Criteria criteria = this.GetCriteriaByName(panelId, crit.Key);
            //FIND CRITERIA BASED ON THE KEY 
            CriteriaValue value = this.GetCriteriaValueBasedOnCriteriaAndValue(criteria.CriteriaId, crit.Value);

            if (criteria != null && value != null)
            {
                CriteriaAnswer criteriaAnswer = new CriteriaAnswer(criteria, value);
                criteriaAnswers.Add(criteriaAnswer);
            }
        }
        
        
        var existingCriteriaGroup = _repo.ReadCriteriaGroupForPanel(panelId, groupName);
        if (existingCriteriaGroup != null)
        {
            
            existingCriteriaGroup.PanelMembers.Add(member);
            _repo.UpdateCriteriaGroup(existingCriteriaGroup);
            _logger.Log(LogLevel.Information, string.Format("Member {0} added to criteriaGroup {1}",member.PanelMemberId, groupName));
        }else{
            _logger.Log(LogLevel.Information, string.Format("Creating new criteriagroup for {0}",groupName));
            var newCriteriaGroup = new CriteriaGroup
            {
                Name = groupName,
                CriteriaAnswers = criteriaAnswers,
            };
            newCriteriaGroup.PanelMembers.Add(member);
            _repo.CreateCriteriaGroup(newCriteriaGroup);

        }
        
        return _repo.ReadCriteriaGroupForPanel(panelId, groupName);
    }*/

    public Criteria GetCriteriaByName(Guid panelId,string critName)
    {
        return _repo.ReadCriteriaByName(panelId, critName);
    }
}
