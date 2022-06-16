using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Models;

public class Teacher {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public IdentityUser User { get; set; }
    public List<SchoolSubject> SchoolSubjects { get; set; }
}