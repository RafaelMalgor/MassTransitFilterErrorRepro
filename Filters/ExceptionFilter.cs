using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ITsynch.Suite.MassTransit;

/// <summary>
/// Filter for wrapping BusinessExceptions around MassTransitApplicationException.
/// </summary>
/// <typeparam name="T">The Message Type.</typeparam>
internal class ExceptionFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly ILogger<ExceptionFilter<T>> logger;

    public ExceptionFilter(ILogger<ExceptionFilter<T>> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("ABCBusinessExceptionFilter");
    }

    /// <inheritdoc/>
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        try
        {
            await next.Send(context).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            this.logger.LogError($"Exception caught in filter: {e.Message}");
            this.logger.LogError($"Silently swallowing the execption.");
        }
    }
}
