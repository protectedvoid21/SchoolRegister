namespace SchoolRegister.Models;

public class StudentSubject {
    public int Id { get; set; }
    public SchoolSubject SchoolSubject { get; set; }
    public Student Student { get; set; }
    public List<Grade>? Grades { get; set; }
}