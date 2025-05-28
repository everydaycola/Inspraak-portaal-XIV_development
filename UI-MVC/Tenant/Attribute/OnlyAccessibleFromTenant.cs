namespace UI_MVC.Tenant.Attribute;

/// <summary>
/// The <see cref="OnlyAccessibleFromTenant"/> attribute is used to mark methods or classes that require organisation context.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class OnlyAccessibleFromTenant : System.Attribute
{
    
}