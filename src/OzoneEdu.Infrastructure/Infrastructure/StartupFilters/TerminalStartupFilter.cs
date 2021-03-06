using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using OzonEdu.Infrastructure.Middlewares;

namespace OzonEdu.Infrastructure.StartupFilters
{
    public sealed class TerminalStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<RequestLoggingMiddleware>();
                app.Map("/version", builder => builder.UseMiddleware<VersionMiddleware>());
                app.Map("/ready", builder => builder.UseMiddleware<ReadyMiddleware>());
                app.Map("/live", builder => builder.UseMiddleware<LiveMiddleware>());
                app.MapWhen(c => c.Connection.LocalPort == 5000 && c.Request.Path == "/",
                    builder => builder.UseMiddleware<ForwardToSwaggerMiddleware>());
                next(app);
            };
        }
    }
}