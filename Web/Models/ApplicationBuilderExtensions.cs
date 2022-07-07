namespace SchoolRegister.Models; 

public static class ApplicationBuilderExtensions {
    public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app) {
        var serviceScope = app.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<SchoolRegisterContext>();

        new SchoolRegisterSeeder()
            .SeedAsync(dbContext, serviceScope.ServiceProvider)
            .GetAwaiter()
            .GetResult();

        return app;
    }
}