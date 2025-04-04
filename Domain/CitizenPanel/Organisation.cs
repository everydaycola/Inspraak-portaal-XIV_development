using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Domain.CitizenPanel;

public class Organisation
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string BackgroundColor { get; set; }
    public string BackgroundImage { get; set; }
}

public class AvailableOrganisations
{
    public const string SectionName = nameof(AvailableOrganisations);
    public Organisation[] Organisations { get; set; }
}

public class AvailableOrganisationsSetup(IConfiguration configuration) : IConfigureOptions<AvailableOrganisations>
{
    public void Configure(AvailableOrganisations options)
    {
        configuration.GetSection(AvailableOrganisations.SectionName).Bind(options);
    }
}

public class OrganisationContext
{
    public Organisation organisation { get; set; } = new();
}

public static class OrganisationExtensions
{
    public static IServiceCollection AddOrganisationContext(this IServiceCollection services)
    {
        services.AddScoped<OrganisationContext>();
        services.AddTransient<Organisation>();
    }
}