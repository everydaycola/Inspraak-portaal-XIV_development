using Domain.Admin;
using Domain.GlobalDtos;
using Domain.Interfaces;
using Domain.Question;
using AnswerOptionImpactsDto = Domain.GlobalDtos.AnswerOptionImpactsDto;

namespace BL.Interfaces;

public interface IQuestionManager
{
    //GET
    public ParticipationMethod GetParticipationMethodByName(string name);
    public IEnumerable<Question> GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
    public Question GetQuestionWithAnswerOptionsAndImpactsAndParticipationMethod(int id);
    public ParticipationMethod GetParticipationMethodById(Guid id);
    public IEnumerable<ParticipationMethod> GetAllParticipationMethods();
    //ADD
    public Question AddQuestion(string question);
    public void AddAnswerOptionsWithImpacts(int questionId, string answerText,List<AnswerOptionImpactsDto> answerOptionImpacts);
    void AddParticipationMethod(string viewModelName, string viewModelDescription, string imageFileName);
    //UPDATE
    public void UpdateQuestionWithAnswerOptions(int modelId, string modelQuestionText, List<AnswerOptionDto> toList);
    public void UpdateParticipationMethod(Guid viewModelId, string viewModelName, string viewModelDescription,string imageUri);
    //DELETE
    public void DeleteQuestionWithAnswerOptionsAndImpacts(int questionId);
    public void DeleteParticipationMethod(Guid id);
    
}