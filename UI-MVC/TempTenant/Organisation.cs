using DAL;
using Microsoft.Extensions.Options;

namespace UI_MVC.TempTenant;

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
public static class OrganisationExtensions
{
    public static IServiceCollection AddOrganisationContext(this IServiceCollection services)
    {
        services.AddScoped<OrganisationContext>();
        services.AddTransient<Organisation>(p => p.GetRequiredService<OrganisationContext>().Organisation);
        services.AddSingleton<IConfigureOptions<AvailableOrganisations>, AvailableOrganisationsSetup>();
        return services;
    }
}