using System.Reflection;

namespace AlzaShop.Core.Commands;

public class CommandError
{
    public string Code { get; }

    public string Message { get; }

    public string? PropertyName { get; }

    public string? EntityName { get; }

    public string? Detail { get; set; }

    public CommandError(string code, string message, string? propertyName = null, MemberInfo? entityType = null)
    {
        Code = code;
        Message = message;
        PropertyName = propertyName;
        EntityName = entityType?.Name;
    }
}
