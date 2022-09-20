namespace SchoolRegister.Models; 

public class SchoolRegisterSeeder : ISeeder {
    public async Task SeedAsync(SchoolContext dbContext, IServiceProvider serviceProvider) {
        if (dbContext == null) {
            throw new ArgumentNullException(nameof(dbContext));
        }
        if (serviceProvider == null) {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        var seeders = new List<ISeeder> {
            new RolesSeeder(),
            new AdminSeeder(),
        };

        foreach (var seeder in seeders) {
            await seeder.SeedAsync(dbContext, serviceProvider);
            await dbContext.SaveChangesAsync();
        }
    }
}