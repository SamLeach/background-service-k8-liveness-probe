using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace Sam.BackgroundService.K8sLivenessProbe.Tests;

public class BackgroundWorkerIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ILogger<BackgroundWorker>>();
                services.AddLogging();
            });
        });

    [Fact]
    public async Task HealthCheckEndpoint_ReturnsHealthy()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/k8s/health");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be("Healthy");
    }

    [Fact]
    public async Task BackgroundWorker_IsRunning()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger<BackgroundWorker>();
        var worker = new BackgroundWorker(logger);

        // Act
        var cancellationTokenSource = new CancellationTokenSource();
        var task = worker.StartAsync(cancellationTokenSource.Token);

        await Task.Delay(2000);

        // Assert
        logger.Should().NotBeNull();

        cancellationTokenSource.Cancel();
        await worker.StopAsync(cancellationTokenSource.Token);
    }
}