namespace Data.Models;

public interface ISeeder {
    Task SeedAsync(SchoolContext dbContext, IServiceProvider serviceProvider);
}