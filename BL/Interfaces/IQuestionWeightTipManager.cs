using Domain.Admin;
using Domain.Interfaces.Question;

namespace BL.Interfaces;

public interface IQuestionWeightTipManager
{
    public List<QuestionWeightTips> GetAllQuestionWeightTips();
    
    public IEnumerable<ParticipationMethod> GetAllParticipationMethods();
    public QuestionWeightTips GetQuestionWeightTip(int id);
    public void AddQuestionWeightTip(int id, int minScore, int maxScore, string messqge);
    
    void AddParticipationMethod(string viewModelName, string viewModelDescription);
    public void UpdateQuestionWeightTip(int id, int minScore, int maxScore, string messqge);
    public void RemoveQuestionWeightTip(int id);
}