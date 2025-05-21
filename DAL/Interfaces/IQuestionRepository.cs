using Domain.Interfaces;

namespace DAL.Repositories;

public interface IQuestionRepository
{
    IEnumerable<Question> ReadAllQuestions();
    Question ReadQuestionById(int id);
    Question CreateQuestion(Question question);
    Question UpdateQuestion(Question question);
    void DeleteQuestion(int id);
}