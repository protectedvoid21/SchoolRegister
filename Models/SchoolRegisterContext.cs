using Microsoft.EntityFrameworkCore;

namespace SchoolRegister.Models; 

public class SchoolRegisterContext : DbContext {
    public SchoolRegisterContext(DbContextOptions<SchoolRegisterContext> options) : base(options) {}

    public DbSet<SchoolClass> SchoolClasses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Subject> Subjects { get; set; }
}