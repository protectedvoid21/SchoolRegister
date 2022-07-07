using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Models;

public class Teacher {
    public int Id { get; set; }
    public AppUser User { get; set; }
    public List<SchoolSubject>? SchoolSubjects { get; set; }
}