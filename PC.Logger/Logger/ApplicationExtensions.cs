using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Util;

namespace PC.Logger.Logger;

/// <summary>
/// ApplicationExtensions
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Конфигурация трейсинга
    /// </summary>
    [Obsolete("Более не поддержвается, не валидное использование Jaeger, используйте метод без передачи параметров")]
    public static void ConfigureTracer(this IServiceCollection services, string serviceName, string jaegerUrl)
    {
        services.AddOpenTracing();
        // Adds the Jaeger Tracer.
        services.AddSingleton<ITracer>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var sampler = new ConstSampler(true);
            var tracer = new Tracer.Builder(serviceName)
                .WithReporter(
                    new RemoteReporter.Builder()
                        .WithLoggerFactory(loggerFactory)
                        .WithSender(new UdpSender(jaegerUrl, 6831, 100))
                        .Build())
                .WithLoggerFactory(loggerFactory)
                .WithSampler(sampler)
                .Build();

            if (!GlobalTracer.IsRegistered())
                GlobalTracer.Register(tracer);

            return tracer;
        });
            
        services.AddOpenTracing(builder =>
        {
            builder.ConfigureAspNetCore(options =>
            {
                // This example shows how to ignore certain requests to prevent spamming the tracer with irrelevant data
                options.Hosting.IgnorePatterns.Add(request => request.Request.Path.Value?.StartsWith("/healthz") == true);
            });
        });

        services.Configure<HttpHandlerDiagnosticOptions>(options =>
        {
            // Prevent endless loops when OpenTracing is tracking HTTP requests to Jaeger. Not effective when UdpSender is used.
            options.IgnorePatterns.Add(request => new Uri($"{jaegerUrl}:14268/api/traces").IsBaseOf(request.RequestUri!));
        });
            
        services.Configure<HttpHandlerDiagnosticOptions>(options =>
            options.OperationNameResolver =
                request => $"{request.Method.Method}: {request.RequestUri?.AbsoluteUri}");
    }
    
     
    /// <summary>
    /// Конфигурация трейсинга
    /// </summary>
    public static void ConfigureTracer(this IServiceCollection services, string serviceName)
    {
        services.AddOpenTracing();
        // Adds the Jaeger Tracer.
        services.AddSingleton<ITracer>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var sampler = new ConstSampler(true);
            var tracer = new Tracer.Builder(serviceName)
                .WithReporter(
                    new RemoteReporter.Builder()
                        .WithLoggerFactory(loggerFactory)
                        .Build())
                .WithLoggerFactory(loggerFactory)
                .WithSampler(sampler)
                .Build();

            if (!GlobalTracer.IsRegistered())
                GlobalTracer.Register(tracer);

            return tracer;
        });
            
        services.AddOpenTracing(builder =>
        {
            builder.ConfigureAspNetCore(options =>
            {
                // This example shows how to ignore certain requests to prevent spamming the tracer with irrelevant data
                options.Hosting.IgnorePatterns.Add(request => request.Request.Path.Value?.StartsWith("/healthz") == true);
            });
        });

        services.Configure<HttpHandlerDiagnosticOptions>(_ => { });
            
        services.Configure<HttpHandlerDiagnosticOptions>(options =>
            options.OperationNameResolver =
                request => $"{request.Method.Method}: {request.RequestUri?.AbsoluteUri}");
    }
}
