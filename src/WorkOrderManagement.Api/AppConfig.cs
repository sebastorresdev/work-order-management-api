using WorkOrderManagement.Infrastructure.Data;
using WorkOrderManagement.Api.Common.Extensions;

using Scalar.AspNetCore;


namespace WorkOrderManagement.Api;


public static class AppConfig
{
    public static async Task AddConfigAsync(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            await app.InitialiseDatabaseAsync();

            app.MapOpenApi("/api/openapi/{documentName}.json");

            app.MapScalarApiReference(options =>
            {
                options
                .WithTitle("SKVIA Attendance — API Docs")
                .WithTheme(ScalarTheme.Laserwave)
                .AddDocument("v1", "Versión 1", routePattern: "/api/openapi/{documentName}.json")
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

            });
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseRequestTimeouts();

        app.UseRateLimiter();

        app.UseOutputCache();

        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<WorkOrderManagement.Api.Common.Middleware.UserContextLoggingMiddleware>();

        app.Map("/", () => Results.Redirect("/scalar"));

        app.MapDefaultEndpoints();

        app.MapEndpoints(typeof(Program).Assembly);
    }
}

