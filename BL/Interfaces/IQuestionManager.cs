using Domain.Interfaces;

namespace BL.Interfaces;

public interface IQuestionManager
{
    IEnumerable<Question> GetAllQuestions();
    Question GetQuestionById(int id);
    void AddQuestion(int id, string question, ICollection<AnswerOption> answerOption);
    void UpdateQuestion(int id, string question, ICollection<AnswerOption> answerOption);
    void RemoveQuestion(int id);
}