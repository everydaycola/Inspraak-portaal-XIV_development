using Domain.Admin;
using Domain.GlobalDtos;
using Domain.Interfaces;
using Domain.Interfaces.Question;
using UI_MVC.Models.ViewModels.ExploreConceptViewModels;
using AnswerOptionImpactsDto = Domain.GlobalDtos.AnswerOptionImpactsDto;

namespace BL.Interfaces;

public interface IQuestionManager
{
    public IEnumerable<Question> GetAllQuestions();
    public ParticipationMethod GetParticipationMethodByName(string name);
    public IEnumerable<Question> GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
    public Question GetQuestionWithAnswerOptionsAndImpactsAndParticipationMethod(int id);
    void RemoveQuestionWithAnswerOptionsAndImpacts(int questionId);
    public Question GetQuestionById(int id);
    public Question AddQuestion(string question);

    public void AddAnswerOptionsWithImpacts(int questionId, string answerText,List<AnswerOptionImpactsDto> answerOptionImpacts);
    void UpdateQuestion(int id, string question, List<AnswerOption> answerOption);
    public void UpdateQuestionWithAnswerOptions(int modelId, string modelQuestionText, List<AnswerOptionDto> toList);
    void RemoveQuestion(int id);
    public void RemoveParticipationMethod(Guid id);
    
}