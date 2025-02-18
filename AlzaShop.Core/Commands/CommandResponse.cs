namespace AlzaShop.Core.Commands;

public class CommandResponse<T>
{
    public T? Result { get; set; }

    public List<CommandError> Errors { get; }

    public bool IsValid => Errors.Count == 0;

    public CommandResponse()
    {
        Errors = new List<CommandError>();
    }

    public CommandResponse(T result)
    {
        Errors = new List<CommandError>();
        Result = result;
    }

    public void AddError(string code, string message)
    {
        AddError(code, message, null);
    }

    public void AddError(string code, string message, string? propertyName)
    {
        Errors.Add(new CommandError(code, message, propertyName));
    }

    public static CommandResponse<T> EndWithError(string code, string message)
    {
        CommandResponse<T> commandResponse = new CommandResponse<T>();
        commandResponse.AddError(code, message);
        return commandResponse;
    }

    public static CommandResponse<T> EndWithError(string code, string message, T result)
    {
        CommandResponse<T> commandResponse = new CommandResponse<T>();
        commandResponse.Result = result;
        commandResponse.AddError(code, message);
        return commandResponse;
    }
}
