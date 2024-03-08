
using Alpha.Gateway.Authentication;
using Alpha.Gateway.OpenTelemetry;
using Alpha.Utils.Consul;
using Gateway.OpenTelemetry;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace Alpha.Gateway;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOcelot()
//                .AddDelegatingHandler<AlphaAuthenticationDelegatinHandler>(true)
            .AddConsul();
        services.AddScoped<AlphaAuthenticationDelegatinHandler>();

        services.ConsulServicesConfig(configuration.GetSection("Consul").Get<ConsulConfig>()!);

        services.AddMemoryCache();
        services.AddHealthChecks();

        // var collectorUri = new Uri("http://collector:4317");

        // services.AddOpenTelemetry()
        //     .ConfigureResource( resourcebuilder => 
        //         resourcebuilder.AddService(DiagnosticsConfig.ServiceName))
        //     .WithTracing(tracerProviderBuilder =>
        //         tracerProviderBuilder
        //             .AddSource(DiagnosticsConfig.ServiceName)
        //             .AddAspNetCoreInstrumentation(options =>
        //             {
        //                 options.Filter = (httpContext) => !httpContext.Request.Path.Equals("/health", StringComparison.OrdinalIgnoreCase);
        //             })
        //             .AddConsoleExporter()
        //             .AddOtlpExporter(options =>
        //             {
        //                 options.Endpoint = collectorUri;
        //                 options.Protocol = OtlpExportProtocol.Grpc;
        //             }))
        //     .WithMetrics( metricsBuilder => {
        //         metricsBuilder.AddAspNetCoreInstrumentation()
        //         .AddProcessInstrumentation()
        //         .AddRuntimeInstrumentation()
        //         .AddMeter(DiagnosticsConfig.Meter.Name).AddOtlpExporter(options =>
        //         {
        //             options.Endpoint = collectorUri;
        //             options.Protocol = OtlpExportProtocol.Grpc;
        //         });
        //     });

        // services.AddLogging( l => {
        //     l.AddOpenTelemetry(o => {
        //         o.SetResourceBuilder( 
        //             ResourceBuilder.CreateDefault().AddService(DiagnosticsConfig.ServiceName))
        //                 .AddOtlpExporter(options =>
        //                 {
        //                     options.Endpoint = collectorUri;
        //                     options.Protocol = OtlpExportProtocol.Grpc;
        //                 });
        //     });
        // });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseMiddleware<RequestIdMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync(DiagnosticsConfig.ServiceName);
            });
        });

//            app.UseOpenTelemetryPrometheusScrapingEndpoint();

        app.UseOcelot();
    }
}
