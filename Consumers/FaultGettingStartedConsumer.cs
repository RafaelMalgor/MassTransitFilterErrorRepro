namespace GettingStarted.Consumers;

using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

public class FaultGettingStartedConsumer : IConsumer<Fault<GettingStarted>>
{
    readonly ILogger<FaultGettingStartedConsumer> _logger;

    public FaultGettingStartedConsumer(ILogger<FaultGettingStartedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Fault<GettingStarted>> context)
    {
        _logger.LogInformation("Fault of GettingStarted Consumed");

        return Task.CompletedTask;
    }
}