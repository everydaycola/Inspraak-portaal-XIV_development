using Domain.Admin;
using Domain.Interfaces.Question;

namespace DAL.Interfaces;

public interface IQuestionWeightTipsRepository
{
    public ParticipationMethod ReadParticipationMethodById(Guid id);
    public List<QuestionWeightTips> ReadAllQuestionWeightTips();
    public QuestionWeightTips ReadQuestionWeightTip(int id);
    
    public IEnumerable<ParticipationMethod> ReadAllParticipationMethods();
    public void CreateQuestionWeightTip(QuestionWeightTips questionWeightTips);
    
    public void CreateParticipationMethod(ParticipationMethod method);
    public void UpdateQuestionWeightTip(QuestionWeightTips questionWeightTips);
    public void UpdateParticipationMethod(ParticipationMethod updatedParticipationMethod);
    public void DeleteQuestionWeightTip(int id);
}