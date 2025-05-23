using Domain;
using Domain.Interfaces.Posts;

namespace UI_MVC.Models.ViewModels;

public class TimeLineViewModel
{
    public ApplicationUser CurrentUser { get; set; }
    public Guid PanelId { get; set; }
    public Guid TimeLineId { get; set; }
    public ICollection<Post> Posts { get; set; }
}