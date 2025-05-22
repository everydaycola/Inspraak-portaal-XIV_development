using BL.Interfaces;
using DAL.Repositories;
using Domain.Interfaces;
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

    public Question GetQuestionById(int id)
    {
        return _questionRepository.ReadQuestionById(id);
    }

    public void AddQuestion(int id, string question, List<AnswerOption> answerOption)
    {
        Question newQuestion = new Question()
        {
            Id = id,
            QuestionText = question,
            AnswerOptions = answerOption
        };
        _questionRepository.CreateQuestion(newQuestion);
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
}