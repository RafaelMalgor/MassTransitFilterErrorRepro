namespace GettingStarted;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using MassTransit.Introspection;
using MassTransit.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Worker : BackgroundService
{
    readonly IBus _bus;
    readonly ILogger<Worker> _logger;

    public Worker(IBus bus, ILogger<Worker> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(JsonSerializer.Serialize(
                 this._bus.GetProbeResult(),
                 typeof(ProbeResult),
                 SystemTextJsonMessageSerializer.Options));

        // Publish a message.
        await _bus.Publish(new GettingStarted { Value = $"The time is {DateTimeOffset.Now}" }, stoppingToken);
    }
}