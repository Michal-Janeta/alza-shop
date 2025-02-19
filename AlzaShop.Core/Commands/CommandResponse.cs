namespace AlzaShop.Core.Commands;

public class CommandResponse<T>
{
    public T? Result { get; set; }

    public List<CommandError> Errors { get; }

    public bool IsValid => !Errors.Any();

    public CommandResponse()
    {
        Errors = new List<CommandError>();
    }

    public CommandResponse(T result)
    {
        Errors = new List<CommandError>();
        Result = result;
    }

    private CommandResponse(CommandError error)
    {
        Errors.Add(error);
    }

    private CommandResponse(IEnumerable<CommandError> errors)
    {
        Errors.AddRange(errors);
    }

    public static CommandResponse<T> Success(T data)
        => new CommandResponse<T>(data);

    public static CommandResponse<T> Error(CommandError error)
        => new CommandResponse<T>(error);

    public static CommandResponse<T> Error(IEnumerable<CommandError> errors)
        => new CommandResponse<T>(errors);
}
