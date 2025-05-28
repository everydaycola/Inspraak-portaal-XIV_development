namespace Domain;

//Interface used for classes that need coupling to an Organisation(Tenant).
public interface IOrganisational
{
    string OrganisationId { get; set; }
}