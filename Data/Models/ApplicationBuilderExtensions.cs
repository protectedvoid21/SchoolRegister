using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Models;

public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app) {
        var serviceScope = app.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<SchoolContext>();

        new SchoolRegisterSeeder()
            .SeedAsync(dbContext, serviceScope.ServiceProvider)
            .GetAwaiter()
            .GetResult();

        return app;
    }
}