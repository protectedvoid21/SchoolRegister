namespace SchoolRegister.Models; 

public interface ISeeder {
    Task SeedAsync(SchoolRegisterContext dbContext, IServiceProvider serviceProvider);
}