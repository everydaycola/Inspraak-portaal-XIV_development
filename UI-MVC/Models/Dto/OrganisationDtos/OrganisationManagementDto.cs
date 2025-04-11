using DAL;

namespace UI_MVC.Models.Dto;

public class OrganisationManagementDto
{
    public IEnumerable<Organisation> Organisations { get; set; }
    public int AmountOfOrganisations { get; set; }
}