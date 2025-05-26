using Domain.Admin;
using Domain.GlobalDtos;
using Domain.Interfaces;
using Domain.Interfaces.Question;

namespace BL.Interfaces;

public interface IQuestionManager
{
    IEnumerable<Question> GetAllQuestions();
    public ParticipationMethod GetParticipationMethodByName(string name);
    IEnumerable<Question> GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
    Question GetQuestionById(int id);
    Question AddQuestion(string question);

    public void AddAnswerOptionsWithImpacts(int questionId, string answerText,List<AnswerOptionImpactsDto> answerOptionImpacts);
    void UpdateQuestion(int id, string question, List<AnswerOption> answerOption);
    void RemoveQuestion(int id);
    public void RemoveParticipationMethod(Guid id);
    
}