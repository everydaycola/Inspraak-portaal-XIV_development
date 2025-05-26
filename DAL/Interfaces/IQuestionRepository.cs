using Domain.Admin;
using Domain.Interfaces;
using Domain.Interfaces.Question;

namespace DAL.Repositories;

public interface IQuestionRepository
{
    IEnumerable<Question> ReadAllQuestions();
    
    IEnumerable<Question> ReadAllQuestionsWIthAnswerOptionsAndIMpactsAndParticipationMethod();
    Question ReadQuestionWithAnswerOptionsById(int id);
    
    public ParticipationMethod ReadParticipationMethodByName(string name);
    public Question CreateQuestion(Question question);
    public void CreateAnswerOptionsWithImpacts(AnswerOption newAnswerOption);
    public void DeleteQuestionWithAnswerOptionsAndImpacts(int questionId);
    void UpdateQuestion(Question question);
    void DeleteQuestion(int id);
    void DeleteParticipationMethod(Guid id);
    
}