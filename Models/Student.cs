using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Models;

public class Student {
    public int Id { get; set; }
    public AppUser User { get; set; }

    public SchoolClass SchoolClass { get; set; }
    public int SchoolClassId { get; set; }
    public List<StudentSubject> StudentSubjects { get; set; }
}