using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ITsynch.Suite.MassTransit;
using Serilog;
using GettingStarted.Consumers;

namespace GettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((host, log) =>
            {
                if (host.HostingEnvironment.IsProduction())
                    log.MinimumLevel.Information();
                else
                    log.MinimumLevel.Debug();
                log.WriteTo.Console();
            })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        x.AddConsumer<GettingStartedConsumer>();
                        x.AddConsumer<FaultGettingStartedConsumer>();

                        x.UsingInMemory((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                            cfg.UseConsumeFilter(typeof(ExceptionFilter<>), context);
                            cfg.UseMessageScope(context);
                            cfg.UseInMemoryOutbox();
                        });

                    });

                    services.AddMassTransitHostedService(true);
                    services.AddHostedService<Worker>();
                });
    }
}
