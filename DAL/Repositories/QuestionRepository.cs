using DAL.EF;
using DAL.Interfaces;
using Domain.Admin;
using Domain.Interfaces;
using Domain.Question;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly CitizenPanelDbContext _context;

    public QuestionRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }

    //READ
    public IEnumerable<Question> ReadAllQuestionsWithAnswerOptionsAndImpactsAndParticipationMethod()
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

    public ParticipationMethod ReadParticipationMethodById(Guid id)
    {
        return _context.ParticipationMethods.Find(id);
    }

    public IEnumerable<ParticipationMethod> ReadAllParticipationMethods()
    {
        return _context.ParticipationMethods.ToList();
    }

    //CREATE
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

    public void CreateParticipationMethod(ParticipationMethod method)
    {
        _context.ParticipationMethods.Add(method);
        _context.SaveChanges();
    }

    //UPDATE
    public void UpdateQuestionWithAnswerOptionsAndImpacts(Question updatedQuestion)
    {
        var existingQuestion = _context.Questions
            .Include(q => q.AnswerOptions)
            .ThenInclude(ao => ao.Impacts)
            .ThenInclude(i => i.ParticipationMethod)
            .FirstOrDefault(q => q.Id == updatedQuestion.Id);

        if (existingQuestion == null) return;
        existingQuestion.QuestionText = updatedQuestion.QuestionText;

        var updatedAnswerOptions = updatedQuestion.AnswerOptions.ToList();
        
        foreach (var updatedAnswer in updatedAnswerOptions)
        {
            var existingAnswer = existingQuestion.AnswerOptions
                .FirstOrDefault(a => a.Id != 0 && a.Id == updatedAnswer.Id);
            if (existingAnswer != null)
            {
                existingAnswer.AnswerOptionText = updatedAnswer.AnswerOptionText;

                var updatedImpacts = updatedAnswer.Impacts.ToList();
                
                foreach (var updatedImpact in updatedImpacts)
                {
                    var existingImpact = existingAnswer.Impacts
                        .FirstOrDefault(i =>
                            i.ParticipationMethod.Id == updatedImpact.ParticipationMethod.Id);

                    if (existingImpact != null)
                    {
                        existingImpact.ImpactWeight = updatedImpact.ImpactWeight;
                    }
                    else
                    {
                        var participationMethod = _context.ParticipationMethods
                            .Find(updatedImpact.ParticipationMethod.Id);
                        existingAnswer.Impacts.Add(new AnswerOptionImpact
                        {
                            ParticipationMethod = participationMethod,
                            ImpactWeight = updatedImpact.ImpactWeight
                        });
                    }
                }
            }
            else
            {
                var newAnswer = new AnswerOption
                {
                    AnswerOptionText = updatedAnswer.AnswerOptionText,
                    Impacts = updatedAnswer.Impacts.Select(i =>
                    {
                        var pm = _context.ParticipationMethods.Find(i.ParticipationMethod.Id);
                        return new AnswerOptionImpact
                        {
                            ParticipationMethod = pm,
                            ImpactWeight = i.ImpactWeight
                        };
                    }).ToList()
                };
                existingQuestion.AnswerOptions.Add(newAnswer);
            }
        }
        
        _context.SaveChanges();
    }

    public void UpdateParticipationMethod(ParticipationMethod updatedParticipationMethod)
    {
        var existingMethod = _context.ParticipationMethods.Find(updatedParticipationMethod.Id);
        if (existingMethod != null)
        {
            existingMethod.Name = updatedParticipationMethod.Name;
            existingMethod.Description = updatedParticipationMethod.Description;
            _context.SaveChanges();
        }
    }

    //DELETE
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