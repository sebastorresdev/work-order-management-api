namespace Skvia.BaseTemplate.Application.Common.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class HasPermissionAttribute(string permission) : Attribute
{
    public string Permission { get; } = permission;
}
