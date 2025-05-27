using Domain.Admin;
using Domain.Interfaces;
using Domain.Interfaces.Question;

namespace DAL.Repositories;

public interface IQuestionRepository
{
    public IEnumerable<Question> ReadAllQuestions();
    public IEnumerable<Question> ReadAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
    public Question ReadQuestionWithAnswerOptionsById(int id);
    public Question ReadQuestionWithAnswerOptionsAndImpactsById(int id);
    public ParticipationMethod ReadParticipationMethodByName(string name);
    public Question CreateQuestion(Question question);
    public void CreateAnswerOptionsWithImpacts(AnswerOption newAnswerOption);
    public void DeleteQuestionWithAnswerOptionsAndImpacts(int questionId);
    void UpdateQuestion(Question question);
    public void UpdateQuestionWithAnswerOptionsAndImpacts(Question updatedQuestion);
    void DeleteQuestion(int id);
    void DeleteParticipationMethod(Guid id);
    
}