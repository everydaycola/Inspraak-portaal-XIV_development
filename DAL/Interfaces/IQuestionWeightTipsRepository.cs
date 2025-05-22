using Domain.Interfaces.Question;

namespace DAL.Interfaces;

public interface IQuestionWeightTipsRepository
{
    public List<QuestionWeightTips> ReadAllQuestionWeightTips();
    public QuestionWeightTips ReadQuestionWeightTip(int id);
    public void CreateQuestionWeightTip(QuestionWeightTips questionWeightTips);
    public void UpdateQuestionWeightTip(QuestionWeightTips questionWeightTips);
    public void DeleteQuestionWeightTip(int id);
}