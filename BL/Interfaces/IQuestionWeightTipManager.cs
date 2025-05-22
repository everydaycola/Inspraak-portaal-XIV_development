using Domain.Interfaces.Question;

namespace BL.Interfaces;

public interface IQuestionWeightTipManager
{
    public List<QuestionWeightTips> GetAllQuestionWeightTips();
    public QuestionWeightTips GetQuestionWeightTip(int id);
    public void AddQuestionWeightTip(int id, int minScore, int maxScore, string messqge);
    public void UpdateQuestionWeightTip(int id, int minScore, int maxScore, string messqge);
    public void RemoveQuestionWeightTip(int id);
}