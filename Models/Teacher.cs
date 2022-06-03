namespace SchoolRegister.Models;

public class Teacher {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<SchoolClass> ClassList { get; set; } = new();
}