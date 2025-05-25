using Domain.Interfaces;
using Domain.Interfaces.Question;

namespace BL.Interfaces;

public interface IQuestionManager
{
    IEnumerable<Question> GetAllQuestions();
    IEnumerable<Question> GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
    Question GetQuestionById(int id);
    void AddQuestion(int id, string question, List<AnswerOption> answerOption);
    
    void AddAnswerOptionImpact(int impactweight, string participationMethodName);
    void UpdateQuestion(int id, string question, List<AnswerOption> answerOption);
    void RemoveQuestion(int id);
    public void RemoveParticipationMethod(Guid id);
}