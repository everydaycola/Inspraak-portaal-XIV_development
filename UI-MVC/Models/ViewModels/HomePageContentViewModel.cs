using Microsoft.Build.Framework;

namespace UI_MVC.Models.ViewModels;

public class HomePageContentViewModel
{
    [Required]
    public string DiscoverConceptText { get; set; }
    [Required]
    public string AboutInspraakPortaalText { get; set; }
}