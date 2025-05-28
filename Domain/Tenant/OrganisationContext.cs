using Domain.Tenant;

namespace DAL;

public class OrganisationContext
{
    public Organisation Organisation { get; set; } = new();
}