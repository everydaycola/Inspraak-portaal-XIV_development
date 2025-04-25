using System.ComponentModel.DataAnnotations;
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
    
    // public Panel GetAllCriteriaWithValuesForPanel(Guid panelId)
    // {
    //     return _repo.ReadAllCriteriaWithValuesForPanel(panelId);
    // }

    public Dictionary<string, Dictionary<string, int>> GetAllCriteriaCountsGroupedByValue(Guid panelId)
    {
        // this gives the exact counts but a criteria isn't present when it is 0
        var counts = _repo.ReadAllCriteriaMemberCountsWithValuesForPanel(panelId);
        // this gives all criteria values, including the ones that are 0
        var all = _repo.ReadAllCriteriaForPanelWithAnswerOptions(panelId)
            .GroupBy(c => c.Name)
            .ToDictionary(
                group => group.Key,
                group => group
                    .SelectMany(c => c.AnswerOptions
                        .Select(ao => ao.Option))
            );
        // we have to merge the two so all criteria have a value, even if it is 0
        return all.ToDictionary(
            criteriaEntry => criteriaEntry.Key,
            criteriaEntry => criteriaEntry.Value.ToDictionary(
                option => option,
                option => counts.TryGetValue(criteriaEntry.Key, out var criteriaValues) && 
                          criteriaValues.TryGetValue(option, out var count) ? count : 0
            )
        );
    }
    
    // public IEnumerable<Criteria> GetAllNonDefaultCriteriaWithValuesForPanel(Guid panelId)
    // {
    //     return _repo.ReadAllNonDefaultCriteriaWithValuesForPanel(panelId);
    // }
    
    // public Dictionary<string, ICollection<PanelMember>> GetPanelMembersGroupedByResponses(Guid panelId)
    // {
    //     var result = new Dictionary<string, ICollection<PanelMember>>();
    //     var panelMembers = _panelManager.GetAllPanelMembersForPanel(panelId);
    //     foreach (var member in panelMembers)
    //     {
    //         var groupName = string.Join("-", member.Responses.OrderBy(r => r.Criteria.Name).Select(r => r.SelectedOption).ToList());
    //         if (!result.TryGetValue(groupName, out var value))
    //         {
    //             value = new List<PanelMember>();
    //             result[groupName] = value; 
    //         }
    //
    //         value.Add(member);
    //     }
    //     return result;
    // }
    
    // public Dictionary<string, ICollection<PanelMember>> GetPanelMembersWithCompletedCriteriaGroupedByResponse(Guid panelId)
    // {
    //     var result = new Dictionary<string, ICollection<PanelMember>>();
    //     var panelMembers = _panelManager.GetAllPanelMembersWhichAnsweredAllQuestionsWithCriteria(panelId);
    //     foreach (var member in panelMembers)
    //     {
    //         var groupName = string.Join("-", member.Responses.OrderBy(r => r.Criteria.Name).Select(r => r.SelectedOption).ToList());
    //         if (!result.TryGetValue(groupName, out var value))
    //         {
    //             value = new List<PanelMember>();
    //             result[groupName] = value; 
    //         }
    //
    //         value.Add(member);
    //     }
    //     return result;
    // }

    public Dictionary<string, Dictionary<int, List<PanelMember>>> GetPanelMembersGroupedByResponsesForDefaultCriteriaGroupedByPhase(Guid panelId)
    {
        var outerResult = new Dictionary<string, Dictionary<int, List<PanelMember>>>();
        var panelMembers = _panelManager.GetAllPanelMembersForPanel(panelId).Where(p => !p.HasRegistered).ToList();
        
        foreach (var member in panelMembers)
        {
            // get group name (key for outer dict)
            var groupName = string.Join("-",
                member.Responses
                    .OrderBy(r => r.Criteria.Name)
                    .Where(r => r.Criteria.IsDefault)
                    .Select(r => r.SelectedOption)
                    .ToList());
            // if key not present, create it
            if (!outerResult.TryGetValue(groupName, out var phaseDict))
            {
                phaseDict = new Dictionary<int, List<PanelMember>>();
                outerResult[groupName] = phaseDict;
            }
            // get phase (key for inner dict)
            var phase = member.Phase;
            // if key not present, create it
            if (!phaseDict.TryGetValue(phase, out var members))
            {
                members = [];
                phaseDict[phase] = members;
            }
            // add the member to this dictionary
            members.Add(member);
        }
        
        // Order each inner dictionary by phase number
        foreach (var groupName in outerResult.Keys.ToList())
        {
            outerResult[groupName] = outerResult[groupName]
                .OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    
        // Order the outer dictionary by groupName
        return outerResult
            .OrderBy(kvp => kvp.Key)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    }
    
    
    // public Criteria GetCriteriaByName(Guid panelId,string critName)
    // {
    //     return _repo.ReadCriteriaByName(panelId, critName);
    // }

    private Criteria GetCriteriaByNameWithAnswerOptions(Guid panelId, string critName)
    {
        return _repo.ReadCriteriaByNameWithAnswerOptions(panelId, critName);
    }

    public Dictionary<string, Dictionary<string, double>> GetAllDesiredCriteriaPercentages(Guid panelId,
        bool onlyDefault = false)
    {
        return _repo.ReadAllCriteriaForPanelWithAnswerOptions(panelId, onlyDefault)
            .ToDictionary(
                criteria => criteria.Name,
                criteria => criteria.AnswerOptions.ToDictionary(
                    option => option.Option,
                    option => option.DistributionPercentage
                )
            );
    }

    public void SavePanelMemberCriteriaResponses(Guid panelId, Dictionary<string, string> CriteriaAnswers,
        PanelMember member)
    {
        foreach (var (criteriaName, selectedOption) in CriteriaAnswers)
        {
            var criteria = GetCriteriaByNameWithAnswerOptions(panelId, criteriaName);
            if (criteria != null)
            {
                var validOptions = criteria.AnswerOptions;
                if (validOptions.Any(option => option.Option == selectedOption))
                {
                    var criteriaResponse = new CriteriaResponse
                    {
                        Criteria = criteria,
                        SelectedOption = selectedOption,
                    };

                    var validationResults = new List<ValidationResult>();

                    if (!Validator.TryValidateObject(criteriaResponse, new ValidationContext(criteriaResponse),
                            validationResults,true))
                        throw new ValidationException(string.Join("\n", validationResults.Select(x => x.ErrorMessage)));

                    member.Responses.Add(criteriaResponse);
                    member.HasRegistered = true;
                    _panelManager.UpdatePanelMember(member);
                }
                else
                {
                    _logger.Log(LogLevel.Critical,
                        "Member " + member.PanelMemberId +
                        " tried inserting an invalid option for a criteria question.");
                }
            }
            _logger.Log(LogLevel.Critical, "Member " + member.PanelMemberId + " tried submitting a non existing criteria.");
        }
    }

    //ADD
    public Criteria AddCriteria(string name, string question, bool isDefault,
        ICollection<CriteriaAnswerOption> answerOptions, bool isDistributionKnown)
    {
        _logger.Log(LogLevel.Information, "Creating criteria with name " + name + "...");
        var criteria = new Criteria
        {
            Name = name,
            IsDefault = isDefault,
            Question = question,
            AnswerOptions = answerOptions,
            IsDistributionKnown = isDistributionKnown
        };
        _logger.Log(LogLevel.Information, "Criteria with name " + criteria.Name + " was created.");
        return criteria;
    }

    public CriteriaAnswerOption AddCriteriaAnswerOption(string option, double distributionPercentage)
    {
        _logger.Log(LogLevel.Information, "Creating criteria answer option with name " + option + "...");
        var cao = new CriteriaAnswerOption
        {
            Option = option,
            DistributionPercentage = distributionPercentage / 100
        };
        _logger.Log(LogLevel.Information, "Criteria answer option with " + cao.Option + " was created.");
        return cao;
    }
}