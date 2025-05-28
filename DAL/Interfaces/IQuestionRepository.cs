using Domain.Admin;
using Domain.Question;

namespace DAL.Interfaces;

public interface IQuestionRepository
{
    //READ
    public IEnumerable<Question> ReadAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
    public Question ReadQuestionWithAnswerOptionsById(int id);
    public Question ReadQuestionWithAnswerOptionsAndImpactsById(int id);
    public ParticipationMethod ReadParticipationMethodByName(string name);
    public ParticipationMethod ReadParticipationMethodById(Guid id);
    public IEnumerable<ParticipationMethod> ReadAllParticipationMethods();
    //CREATE
    public Question CreateQuestion(Question question);
    public void CreateAnswerOptionsWithImpacts(AnswerOption newAnswerOption);
    public void CreateParticipationMethod(ParticipationMethod method);
    //UPDATE
    public void UpdateQuestionWithAnswerOptionsAndImpacts(Question updatedQuestion);
    public void UpdateParticipationMethod(ParticipationMethod updatedParticipationMethod);
    //DELETE
    public void DeleteQuestionWithAnswerOptionsAndImpacts(int questionId);
    void DeleteParticipationMethod(Guid id);
    
}