using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class SchoolContext : IdentityDbContext<AppUser, IdentityRole, string> {
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) {
    }

    public DbSet<SchoolClass> SchoolClasses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Grade> Grades { get; set; }

    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Message> Messages { get; set; }

    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<SchoolSubject> SchoolSubjects { get; set; }
}