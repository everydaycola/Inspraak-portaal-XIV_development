using BL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Domain.Admin;
using Domain.GlobalDtos;
using Domain.Question;
using Microsoft.Extensions.Logging;
using AnswerOptionImpactsDto = Domain.GlobalDtos.AnswerOptionImpactsDto;

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
    //GET
    public ParticipationMethod GetParticipationMethodByName(string name)
    {
        return _questionRepository.ReadParticipationMethodByName(name);
    }
    public IEnumerable<Question> GetAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod()
    {
        return _questionRepository.ReadAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod();
    }
    public Question GetQuestionWithAnswerOptionsAndImpactsAndParticipationMethod(int id)
    {
        return _questionRepository.ReadQuestionWithAnswerOptionsAndImpactsById(id);
    }

    public ParticipationMethod GetParticipationMethodById(Guid id)
    {
        return _questionRepository.ReadParticipationMethodById(id);
    }

    public IEnumerable<ParticipationMethod> GetAllParticipationMethods()
    {
        return _questionRepository.ReadAllParticipationMethods();
    }

    //ADD
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

    public void AddParticipationMethod(string viewModelName, string viewModelDescription, string imageFileName)
    {
        var method = new ParticipationMethod()
        {
            Name = viewModelName,
            Description = viewModelDescription,
            ImageUri = imageFileName
        };
        _questionRepository.CreateParticipationMethod(method);
    }

    //UPDATE
    public void UpdateQuestionWithAnswerOptions(int modelId, string modelQuestionText, List<AnswerOptionDto> toList)
    {
        var updatedQuestion = new Question()
        {
            Id = modelId,
            QuestionText = modelQuestionText,
            AnswerOptions = toList.Select(aodto => new AnswerOption()
            {
                Id = aodto.Id,
                AnswerOptionText = aodto.AnswerText,
                Impacts = aodto.Impacts.Select(idto => new AnswerOptionImpact()
                {
                    ParticipationMethod = GetParticipationMethodByName(idto.ParticipationMethodName),
                    ImpactWeight = idto.ImpactWeight
                }).ToList()
            }).ToList()
        };
        Console.WriteLine(updatedQuestion);

        _questionRepository.UpdateQuestionWithAnswerOptionsAndImpacts(updatedQuestion);
    }

    public void UpdateParticipationMethod(Guid viewModelId,string viewModelName,
        string viewModelDescription, string imageUri)
    {
        ParticipationMethod updatedParticipationMethod = new ParticipationMethod()
        {
            Id = viewModelId,
            Name = viewModelName,
            Description = viewModelDescription,
            ImageUri = imageUri
        };
        _questionRepository.UpdateParticipationMethod(updatedParticipationMethod);
    }

    //DELETE
    public void DeleteQuestionWithAnswerOptionsAndImpacts(int questionId)
    {
        _questionRepository.DeleteQuestionWithAnswerOptionsAndImpacts(questionId);
    }

    public Question GetQuestionById(int id)
    {
        return _questionRepository.ReadQuestionWithAnswerOptionsById(id);
    }

    public Question GetQuestionWithAnswerOptionsAndImpactsById(int id)
    {
        return _questionRepository.ReadQuestionWithAnswerOptionsAndImpactsById(id);
    }
    public void DeleteParticipationMethod(Guid id)
    {
        _questionRepository.DeleteParticipationMethod(id);
    }
}