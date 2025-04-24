using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;
using UI_MVC.Services;

namespace UI_MVC.Controllers;

public class GemeenteController : Controller
{
    private readonly IPinCService _pinCService;

    public GemeenteController(IPinCService pinCService)
    {
        _pinCService = pinCService;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _pinCService.GetPopulationDataAsync();
        var gemeenten = await _pinCService.GetGemeenteNamenAsync();
        var model = values
            .Where(v => gemeenten.ContainsKey(v.ExternalCode))
            .Select(v => new GemeenteViewModel
            {
                GemeenteNaam = gemeenten[v.ExternalCode],
                GemeenteCode = v.ExternalCode,
                AantalInwoners = v.ValueString
            })
            .OrderBy(x => x.GemeenteNaam)
            .ToList();

        return View(model);
    }
}