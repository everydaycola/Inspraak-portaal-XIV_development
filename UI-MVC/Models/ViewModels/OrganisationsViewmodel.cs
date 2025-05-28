using DAL;
using Domain.Tenant;

namespace UI_MVC.Models.ViewModels;

public class OrganisationsViewmodel
{
    public IEnumerable<Organisation> Organisations { get; set; }
    public int AmountOfOrganisations { get; set; }
}