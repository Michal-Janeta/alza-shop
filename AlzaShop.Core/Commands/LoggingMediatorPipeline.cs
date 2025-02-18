using MediatR;
using Serilog;

namespace AlzaShop.Core.Commands;

public class LoggingMediatorPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger logger;

    public LoggingMediatorPipeline(ILogger logger) => this.logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = request.GetType().Name;
        string requestGuid = Guid.NewGuid().ToString();
        string requestNameWithGuid = $"{requestName} [{requestGuid}]";

        logger.Information("[Začátek] {Id}", requestNameWithGuid);
        TResponse response;

        response = await next();
        logger.Information("[Konec] {Id}", requestNameWithGuid);
        return response;
    }
}
