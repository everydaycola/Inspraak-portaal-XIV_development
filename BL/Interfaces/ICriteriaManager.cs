using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICriteriaManager
{
    //GETS
    public Dictionary<string, Dictionary<string, int>> GetAllCriteriaCountsGroupedByValue(Guid panelId, bool onlyUnknown = false);
    public IEnumerable<Criteria> GetAllDesiredCriteriaPercentages(Guid panelId, bool onlyDefault = false);

    public Dictionary<string, Dictionary<int, List<PanelMember>>> GetPanelMembersGroupedByResponsesForDefaultCriteriaGroupedByPhase(
        Guid panelId);
    public List<PanelMember> GetRegisteredPanelMembersOfPanel(Guid panelId);
    
    //SAVES
    public void SavePanelMemberCriteriaResponses(Guid panelId,Dictionary<string, string> CriteriaAnswers, PanelMember member);
    
    //ADD
    public Criteria AddCriteria(string name, string question, bool isDefault, ICollection<CriteriaAnswerOption> answerOptions, bool isDistributionKnown);

    public CriteriaAnswerOption AddCriteriaAnswerOption(string option, double distributionPercentage);
}