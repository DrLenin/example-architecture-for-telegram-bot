using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using PC.Logger.Logger.Http.Middlewares;

namespace PC.Logger.Logger.Http
{
    /// <inheritdoc />
    internal class LoggingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                
                next(app);
            };
        }
    }
}