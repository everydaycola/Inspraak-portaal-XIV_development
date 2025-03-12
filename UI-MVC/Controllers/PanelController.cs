using BL;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers;

public class PanelController : Controller
{
    private readonly IManager _manager;

    public PanelController(IManager manager)
    {
        _manager = manager;
    }

    public IActionResult MakeNewPanel()
    {
        return View();
    }

}