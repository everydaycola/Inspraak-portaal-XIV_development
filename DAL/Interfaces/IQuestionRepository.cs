using Domain.Interfaces;

namespace DAL.Repositories;

public interface IQuestionRepository
{
    IEnumerable<Question> ReadAllQuestions();
    Question ReadQuestionById(int id);
    void CreateQuestion(Question question);
    void UpdateQuestion(Question question);
    void DeleteQuestion(int id);
}