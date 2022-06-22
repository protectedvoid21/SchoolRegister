namespace SchoolRegister.Models; 

public class SchoolSubject {
    public int Id { get; set; }
    public Subject Subject { get; set; }
    public SchoolClass SchoolClass { get; set; }
    public Teacher Teacher { get; set; }
    public List<StudentSubject> StudentSubjects { get; set; }
}