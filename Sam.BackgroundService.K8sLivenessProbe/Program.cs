
namespace Sam.BackgroundService.K8sLivenessProbe;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHostedService<BackgroundWorker>();
        builder.Services.AddHealthChecks();

        var app = builder.Build();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapHealthChecks("/k8s/health");
        });

        app.Run();
    }
}
