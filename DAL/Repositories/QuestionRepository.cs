using DAL.EF;
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

    public Question ReadQuestionById(int id)
    {
        return _context.Questions.Include(q => q.AnswerOptions).FirstOrDefault(q => q.Id == id);
    }

    public void CreateQuestion(Question question)
    {
        _context.Questions.Add(question);
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
}