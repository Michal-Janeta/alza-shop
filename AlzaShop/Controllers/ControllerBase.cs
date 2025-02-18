using AlzaShop.Core;
using AlzaShop.Core.Commands;
using MediatR;
using ILogger = Serilog.ILogger;

namespace AlzaShop.Api.Controllers;

public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected readonly IMediator Mediator;
    protected readonly ILogger Logger;

    public ControllerBase(IMediator mediator, ILogger logger)
    {
        Mediator = mediator;
        Logger = logger;
    }

    protected async Task<CommandResponse<T>> ExecuteCommand<T>(IRequest<CommandResponse<T>> command)
    {
        CommandResponse<T> result;
        try
        {
            result = await Mediator.Send(command);
            if (result == null)
            {
                result = new CommandResponse<T>();
                result.Errors.Add(new CommandError(ErrorCodes.ExceptionOccured, "Command result is null"));
                Logger.Error($"Command {command.GetType()} is null. Command: {command}");
            }
        }
        catch (Exception ex)
        {
            result = new CommandResponse<T>();
            result.Errors.Add(new CommandError(ErrorCodes.ExceptionOccured, "Command failed"));
            Logger.Error(ex, $"Command {command.GetType()} failed. Command: {command} ");
        }

        return result;
    }
}
