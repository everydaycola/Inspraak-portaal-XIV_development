namespace UI_MVC.Models.ViewModels;

public class OrganisationManagementViewModel
{
    public string OrganisationId { get; set; }
    public string Name { get; set; }
    public string BackgroundColor { get; set; }
    
    public IFormFile File { get; set; }
    public IFormFile LogoFile { get; set; }
}