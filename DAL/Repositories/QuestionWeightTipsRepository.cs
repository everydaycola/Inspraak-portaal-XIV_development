using DAL.EF;
using DAL.Interfaces;
using Domain.Interfaces.Question;

namespace DAL.Repositories;

public class QuestionWeightTipsRepository : IQuestionWeightTipsRepository
{
    private readonly CitizenPanelDbContext _context;

    public QuestionWeightTipsRepository(CitizenPanelDbContext context)
    {
        _context = context;
    }

    public List<QuestionWeightTips> ReadAllQuestionWeightTips()
    {
        return _context.QuestionWeightTips.ToList();
    }

    public QuestionWeightTips ReadQuestionWeightTip(int id)
    {
        return _context.QuestionWeightTips.Find(id);
    }

    public void CreateQuestionWeightTip(QuestionWeightTips questionWeightTips)
    {
        _context.QuestionWeightTips.Add(questionWeightTips);
        _context.SaveChanges();
    }

    public void UpdateQuestionWeightTip(QuestionWeightTips questionWeightTips)
    {
        var existingQuestionWeightTip = _context.QuestionWeightTips.Find(questionWeightTips.Id);
        if (existingQuestionWeightTip != null)
        {
            _context.Entry(existingQuestionWeightTip).CurrentValues.SetValues(questionWeightTips);
            _context.SaveChanges();
        }
        _context.QuestionWeightTips.Update(questionWeightTips);
        _context.SaveChanges();
    }

    public void DeleteQuestionWeightTip(int id)
    {
        var questionWeightTipToDelete = _context.QuestionWeightTips.Find(id);
        if (questionWeightTipToDelete != null)
        {
            _context.QuestionWeightTips.Remove(questionWeightTipToDelete);
            _context.SaveChanges();
        }
    }
}