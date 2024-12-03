namespace Sam.BackgroundService.K8sLivenessProbe;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class BackgroundWorker(ILogger<BackgroundWorker> logger) : BackgroundService
{
    private readonly ILogger<BackgroundWorker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BackgroundWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("BackgroundWorker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
        _logger.LogInformation("BackgroundWorker stopped.");
    }
}
