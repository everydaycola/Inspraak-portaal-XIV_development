using BL.Interfaces;
using Domain.Tenant;
using Microsoft.Extensions.Options;

namespace UI_MVC.Tenant;

public class AvailableTenants
{
    public const string SectionName = nameof(AvailableTenants);
    public Organisation[] Organisations { get; set; }
}

public static class TenantExtensions
{
    public static IServiceCollection AddOrganisationContext(this IServiceCollection services)
    {
        services.AddScoped<OrganisationContext>();
        services.AddTransient<Organisation>(p => p.GetRequiredService<OrganisationContext>().Organisation);
        
        services.AddScoped(provider =>
        {
            var manager = provider.GetRequiredService<IOrganisationManager>();
            return new AvailableTenants
            {
                Organisations = manager.GetAllOrganisations().ToArray()
            };
        });

        return services;
    }
}