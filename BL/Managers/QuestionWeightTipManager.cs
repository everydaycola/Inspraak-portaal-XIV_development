using BL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Domain.Admin;
using Domain.Interfaces.Question;
using Microsoft.Extensions.Logging;

namespace BL.Managers;

public class QuestionWeightTipManager : IQuestionWeightTipManager
{
    private readonly ILogger<QuestionWeightTipManager> _logger;
    private readonly IQuestionWeightTipsRepository _questionWeightTipsRepository;

    public QuestionWeightTipManager(ILogger<QuestionWeightTipManager> logger,
        IQuestionWeightTipsRepository questionWeightTipsRepository)
    {
        _logger = logger;
        _questionWeightTipsRepository = questionWeightTipsRepository;
    }

    public List<QuestionWeightTips> GetAllQuestionWeightTips()
    {
        return _questionWeightTipsRepository.ReadAllQuestionWeightTips();
    }

    public IEnumerable<ParticipationMethod> GetAllParticipationMethods()
    {
        return _questionWeightTipsRepository.ReadAllParticipationMethods();
    }

    public QuestionWeightTips GetQuestionWeightTip(int id)
    {
        return _questionWeightTipsRepository.ReadQuestionWeightTip(id);
    }

    public void AddQuestionWeightTip(int id, int minScore, int maxScore, string messqge)
    {
        QuestionWeightTips questionWeightTips = new QuestionWeightTips()
        {
            Id = id,
            MinScore = minScore,
            MaxScore = maxScore,
            Message = messqge
        };
        _questionWeightTipsRepository.CreateQuestionWeightTip(questionWeightTips);
    }

    public void AddParticipationMethod(string viewModelName, string viewModelDescription)
    {
        var method = new ParticipationMethod()
        {
            Name = viewModelName,
            Description = viewModelDescription
        };
        _questionWeightTipsRepository.CreateParticipationMethod(method);
    }

    public void UpdateQuestionWeightTip(int id, int minScore, int maxScore, string messqge)
    {
        QuestionWeightTips questionWeightTips = new QuestionWeightTips()
        {
            Id = id,
            MinScore = minScore,
            MaxScore = maxScore,
            Message = messqge
        };
        _questionWeightTipsRepository.UpdateQuestionWeightTip(questionWeightTips);
    }

    public void RemoveQuestionWeightTip(int id)
    {
        _questionWeightTipsRepository.DeleteQuestionWeightTip(id);
    }
}