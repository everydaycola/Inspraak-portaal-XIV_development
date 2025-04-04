using Microsoft.Extensions.Options;

namespace UI_MVC.TempTenant;

public class TenantMiddleware(OrganisationContext organisationContext, IOptionsSnapshot<AvailableOrganisations> availableTenants)
    : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var subdomain = context.Request.Host.Host.Split('.')[0];

        var matchingTenant = availableTenants.Value.Organisations.FirstOrDefault(t => t.Id == subdomain);
        if (matchingTenant == null)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return Task.CompletedTask;
        }

        organisationContext.Organisation = matchingTenant;
        return next(context);
    }
}