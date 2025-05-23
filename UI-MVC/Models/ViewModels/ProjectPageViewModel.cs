using Domain;
using Domain.CitizenPanel;

namespace UI_MVC.Models.ViewModels;

public class ProjectPageViewModel
{
    // Panel including all posts including suggestions for suggestionposts including the votes on those suggestions.
    public Panel Panel { get; set; }
    public ApplicationUser CurrentUser { get; set; }
}