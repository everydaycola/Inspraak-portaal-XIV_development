using Domain;
using Domain.Interfaces.Posts;

namespace UI_MVC.Models.ViewModels;

public class PostListViewModel
{
    public ApplicationUser CurrentUser { get; set; }
    public Guid PanelId { get; set; }
    public Guid Parent { get; set; }
    public ICollection<Post> Posts { get; set; }
}