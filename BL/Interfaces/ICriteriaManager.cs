using Domain.CitizenPanel;

namespace BL.Interfaces;

public interface ICriteriaManager
{
    //GETS
    public Dictionary<string, Dictionary<string, int>> GetAllCriteriaCountsGroupedByValue(Guid panelId);

    public Dictionary<string, ICollection<PanelMember>> GetPanelMembersGroupedByResponsesForDefaultCriteria(
        Guid panelId);
    public Criteria GetCriteriaByNameWithAnswerOptions(Guid panelId, string critName);
    
    //SAVES
    void SavePanelMemberCriteriaResponses(Guid panelId,Dictionary<string, string> CriteriaAnswers, PanelMember member);
    
    //ADD
    public Criteria AddCriteria(string name, string question, bool isDefault, ICollection<CriteriaAnswerOption> answerOptions);

    public CriteriaAnswerOption AddCriteriaAnswerOption(string option, double distributionPercentage);
}