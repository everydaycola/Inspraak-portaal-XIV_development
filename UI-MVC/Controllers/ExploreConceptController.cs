using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.ViewModels;

namespace UI_MVC.Controllers;

public class ExploreConceptController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ExploreConceptController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var viewModel = new ExploreConceptViewModel
        {
            Questions = new List<QuestionsViewModel>
            {
                new QuestionsViewModel
                {
                    Id = 1, Question = "Heeft uw organisatie behoefte aan brede burgerbetrokkenheid?",
                    Weight = 1.5
                },
                new QuestionsViewModel
                {
                    Id = 2,
                    Question = "Bent u bereid om de aanbevelingen van burgers serieus te overwegen?",
                    Weight = 2.0
                },
                new QuestionsViewModel
                {
                    Id = 3,
                    Question = "Heeft u een concreet vraagstuk waarvoor input van burgers waardevol is?",
                    Weight = 1.0,
                }
            }
        };
        return View(viewModel);
    }
}