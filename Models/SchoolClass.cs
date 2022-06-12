namespace SchoolRegister.Models;

public class SchoolClass {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Student> StudentsList { get; set; } = new();
    public List<SchoolClass> SchoolSubjects { get; set; } = new();
}