namespace AlzaShop.Core.Commands;

public class CommandResponse<T>
{
    public T? Result { get; set; }

    public List<CommandError> Errors { get; }

    public bool IsValid => !Errors.Any();

    private CommandResponse(T result)
    {
        Errors = new List<CommandError>();
        Result = result;
    }

    private CommandResponse(CommandError error)
    {
        Errors = new List<CommandError>
        {
            error
        };
    }

    private CommandResponse(IEnumerable<CommandError> errors)
    {
        Errors = new List<CommandError>(errors);
    }

    public static CommandResponse<T> Success(T data)
        => new CommandResponse<T>(data);

    public static CommandResponse<T> Error(CommandError error)
        => new CommandResponse<T>(error);

    public static CommandResponse<T> Error(IEnumerable<CommandError> errors)
        => new CommandResponse<T>(errors);
}
