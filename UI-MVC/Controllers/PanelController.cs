using BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers;

public class PanelController : Controller
{
    private readonly IPanelManager _manager;

    public PanelController(IPanelManager manager)
    {
        _manager = manager;
    }

    public IActionResult MakeNewPanel()
    {
        return View();
    }

}