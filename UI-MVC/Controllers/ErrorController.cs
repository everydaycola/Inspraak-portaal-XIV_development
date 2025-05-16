using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                return View("NotFound");
            case 500:
                return View("ServerError");
            default:
                return View("GenericError");
        }
    }
    [Route("Error")]
    public IActionResult Error()
    {
        return View("GenericError");
    }
    
}