using Domain.Interfaces;
using Domain.Interfaces.Question;

namespace DAL.Repositories;

public interface IQuestionRepository
{
    IEnumerable<Question> ReadAllQuestions();
    
    IEnumerable<Question> ReadAllQuestionsWIthAnswerOptionsAndIMpactsAndParticipationMethod();
    Question ReadQuestionById(int id);
    void CreateQuestion(Question question);
    void UpdateQuestion(Question question);
    void DeleteQuestion(int id);
}