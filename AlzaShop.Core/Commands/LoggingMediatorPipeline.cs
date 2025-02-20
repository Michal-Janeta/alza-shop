using MediatR;
using Serilog;

namespace AlzaShop.Core.Commands;

public class LoggingMediatorPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger logger;

    public LoggingMediatorPipeline(ILogger logger) => this.logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestDescription = $"{request.GetType().Name} [{Guid.NewGuid().ToString()}]";

        logger.Information("[Request begin] {Id}", requestDescription);
        TResponse response;

        response = await next();
        logger.Information("[Request end] {Id}", requestDescription);
        return response;
    }
}
