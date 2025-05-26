using BL.Interfaces;
using DAL.Repositories;
using Domain.Admin;
using Domain.GlobalDtos;
using Domain.Interfaces;
using Domain.Interfaces.Question;
using Microsoft.Extensions.Logging;

namespace BL.Managers;

public class QuestionManager : IQuestionManager
{
    private readonly ILogger<QuestionManager> _logger;
    private readonly IQuestionRepository _questionRepository;

    public QuestionManager(ILogger<QuestionManager> logger, IQuestionRepository questionRepository)
    {
        _logger = logger;
        _questionRepository = questionRepository;
    }

    public IEnumerable<Question> GetAllQuestions()
    {
        return _questionRepository.ReadAllQuestions();
    }

    public IEnumerable<Question> GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod()
    {
        return _questionRepository.ReadAllQuestionsWIthAnswerOptionsAndIMpactsAndParticipationMethod();
    }

    public Question GetQuestionById(int id)
    {
        return _questionRepository.ReadQuestionById(id);
    }
    
    public ParticipationMethod GetParticipationMethodByName(string name)
    {
        return _questionRepository.ReadParticipationMethodByName(name);
    }

    public Question AddQuestion(string question)
    {
        Question newQuestion = new Question()
        {
            QuestionText = question,
        };
        return _questionRepository.CreateQuestion(newQuestion);
    }

    public void AddAnswerOptionsWithImpacts(int questionId,string answerText, List<AnswerOptionImpactsDto> answerOptionImpacts)
    {

        var question = GetQuestionById(questionId);
        List<AnswerOptionImpact> newImpacts = new List<AnswerOptionImpact>();
        foreach (var option in answerOptionImpacts)
        {
            var participationMethod = GetParticipationMethodByName(option.ParticipationMethodName);
            var answerOptionImpact = new AnswerOptionImpact()
            {
                ImpactWeight = option.ImpactWeight,
                ParticipationMethod = participationMethod
            };
            newImpacts.Add(answerOptionImpact);
        }
        var newAnswerOption = new AnswerOption()
        {
            AnswerOptionText = answerText,
            Impacts = newImpacts,
            Question = question
        };
        _questionRepository.CreateAnswerOptionsWithImpacts(newAnswerOption);
    }
    


    public void UpdateQuestion(int id, string question, List<AnswerOption> answerOption)
    {
        Question newQuestion = new Question()
        {
            Id = id,
            QuestionText = question,
            AnswerOptions = answerOption
        };
        _questionRepository.UpdateQuestion(newQuestion);
    }

    public void RemoveQuestion(int id)
    {
        _questionRepository.DeleteQuestion(id);
    }

    public void RemoveParticipationMethod(Guid id)
    {
        _questionRepository.DeleteParticipationMethod(id);
    }
}