namespace UI_MVC.TempTenant;

public class OrganisationMiddleware(OrganisationContext organisationContext, AvailableOrganisations availableTenants)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var subdomain = context.Request.Host.Host.Split('.')[0];
        var endpoint = context.GetEndpoint();
        var requiresOrg = endpoint?.Metadata.GetMetadata<RequiresOrganisation>();
        
        // If the endpoint requires an organisation, proceed with tenanting logic
        if (requiresOrg != null)
        {
            var matchingTenant = availableTenants.Organisations.FirstOrDefault(t => t.Id == subdomain);
            
            if (matchingTenant == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }
            
            organisationContext.Organisation = matchingTenant;
        }
        else
        {
            var matchingTenant = availableTenants.Organisations.FirstOrDefault(t => t.Id == subdomain);
            
            if (matchingTenant != null)
            {
                organisationContext.Organisation = matchingTenant;
            }
        }
        await next(context);
    }
}