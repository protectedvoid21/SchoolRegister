using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Models;

public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public IdentityUser User { get; set; }

    public SchoolClass Class { get; set; }
    public List<StudentSubject>? StudentSubjects { get; set; }
}