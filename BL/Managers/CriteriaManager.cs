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

    public CriteriaManager(ILogger<CriteriaManager> logger,ICriteriaRepository repo)
    {
        _repo = repo;
        _logger = logger;
    }
    public IEnumerable<Criteria> GetAllCriteriaWithValuesForPanel(Guid panelId)
    {
        return _repo.ReadAllCriteriaWithValuesForPanel(panelId);
    }

    public IEnumerable<Criteria> GetAllDefaultCriteriaWithValuesForPanel(Guid panelId)
    {
        return _repo.ReadAllDefaultCriteriaWithValuesForPanel(panelId);
    }
    
    public IEnumerable<CriteriaGroup> GetAllCriteriaGroupForPanel(Guid panelId)
    {
        return _repo.ReadAllCriteriaGroupForPanel(panelId);
    }

    public CriteriaGroup GetCriteriaGroupByPanelIdAndName(Guid panelId, string groupName)
    {
        return _repo.ReadCriteraGroupByPanelIdAndName(panelId, groupName);
    }
    
    public CriteriaGroup AssignMemberToCriteriaGroup(Guid panelId, Dictionary<string, string> CriteriaAnswers, PanelMember member)
    {
        string groupName = string.Join("-", CriteriaAnswers.Values);
        var existingCriteriaGroup = _repo.ReadCriteriaGroupForPanel(panelId, groupName);
        if (existingCriteriaGroup != null)
        {
            
            existingCriteriaGroup.PanelMembers.Add(member);
            _repo.UpdateCriteriaGroup(existingCriteriaGroup);
            _logger.Log(LogLevel.Information, string.Format("Member {0} added to criteriaGroup {1}",member.PanelMemberId, groupName));
        }else{
            _logger.Log(LogLevel.Information, string.Format("Creating new criteriagroup for {0}",groupName));
            ICollection<Criteria> panelCriteria = _repo.ReadAllCriteriaWithValuesForPanel(panelId) as ICollection<Criteria>;
            var newCriteriaGroup = new CriteriaGroup
            {
                Name = groupName,
                Criteria = panelCriteria,
            };
            newCriteriaGroup.PanelMembers.Add(member);
            _repo.CreateCriteriaGroup(newCriteriaGroup);

        }
        
        return _repo.ReadCriteriaGroupForPanel(panelId, groupName);
    }
    
}
