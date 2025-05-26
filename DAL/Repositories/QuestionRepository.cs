using DAL.EF;
using Domain.Admin;
using Domain.Interfaces;
using Domain.Interfaces.Question;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly CitizenPanelDbContext _context;

    public QuestionRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Question> ReadAllQuestions()
    {
        return _context.Questions.Include(q => q.AnswerOptions).ToList();
    }

    public IEnumerable<Question> ReadAllQuestionsWIthAnswerOptionsAndIMpactsAndParticipationMethod()
    {
        return _context.Questions
            .Include(q => q.AnswerOptions)
            .ThenInclude(ao => ao.Impacts)
            .ThenInclude(aoi => aoi.ParticipationMethod).ToList();
    }

    public Question ReadQuestionWithAnswerOptionsById(int id)
    {
        return _context.Questions.Include(q => q.AnswerOptions).FirstOrDefault(q => q.Id == id);
    }

    public Question ReadQuestionWithAnswerOptionsAndImpactsById(int id)
    {
        return _context.Questions.Include(q => q.AnswerOptions).ThenInclude(ao => ao.Impacts)
            .FirstOrDefault(q => q.Id == id);
    }

    public ParticipationMethod ReadParticipationMethodByName(string name)
    {
        return _context.ParticipationMethods.FirstOrDefault(mp => mp.Name == name);
    }

    public Question CreateQuestion(Question question)
    {
        _context.Questions.Add(question);
        _context.SaveChanges();
        return ReadQuestionWithAnswerOptionsById(question.Id);
    }

    public void CreateAnswerOptionsWithImpacts(AnswerOption newAnswerOption)
    {
        _context.AnswerOptions.Add(newAnswerOption);
        _context.SaveChanges();
    }

    public void DeleteQuestionWithAnswerOptionsAndImpacts(int questionId)
    {
        var question = ReadQuestionWithAnswerOptionsAndImpactsById(questionId);
        foreach (var option in question.AnswerOptions)
        {
            foreach (var impact in option.Impacts)
            {
                _context.AnswerOptionImpacts.Remove(impact);
            }
            _context.AnswerOptions.Remove(option);
        }
        _context.Questions.Remove(question);

        _context.SaveChanges();
    }

    public void UpdateQuestion(Question updatedQuestion)
    {
        var existingQuestion = _context.Questions
            .Include(q => q.AnswerOptions)
            .FirstOrDefault(q => q.Id == updatedQuestion.Id);

        if (existingQuestion != null)
        {
            _context.Entry(existingQuestion).CurrentValues.SetValues(updatedQuestion);
            foreach (var existingOption in
                     existingQuestion.AnswerOptions.ToList())
            {
                if (!updatedQuestion.AnswerOptions.Any(ao => ao.Id == existingOption.Id))
                {
                    _context.AnswerOptions.Remove(existingOption);
                }
            }

            foreach (var updatedOption in updatedQuestion.AnswerOptions)
            {
                var existingOption = existingQuestion.AnswerOptions.FirstOrDefault(ao => ao.Id == updatedOption.Id);
                if (existingOption == null)
                {
                    existingQuestion.AnswerOptions.Add(updatedOption);
                }
                else
                {
                    _context.Entry(existingOption).CurrentValues.SetValues(updatedOption);
                }
            }

            _context.SaveChanges();
        }
    }

    public void DeleteQuestion(int id)
    {
        var questionToDelete = _context.Questions.Find(id);
        if (questionToDelete != null)
        {
            _context.Questions.Remove(questionToDelete);
            _context.SaveChanges();
        }
    }

    public void DeleteParticipationMethod(Guid id)
    {
        var participationMethodToDelete = _context.ParticipationMethods.Find(id);
        if (participationMethodToDelete != null)
        {
            var relatedImpacts = _context.AnswerOptionImpacts
                .Where(impact => impact.ParticipationMethod == participationMethodToDelete)
                .ToList();
            _context.AnswerOptionImpacts.RemoveRange(relatedImpacts);
            _context.ParticipationMethods.Remove(participationMethodToDelete);
            _context.SaveChanges();
        }
    }
}