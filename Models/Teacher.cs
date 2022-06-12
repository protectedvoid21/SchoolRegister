namespace SchoolRegister.Models;

public class Teacher {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<SchoolSubject> SchoolSubjects { get; set; }
}