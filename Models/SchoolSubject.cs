namespace SchoolRegister.Models; 

public class SchoolSubject {
    public int Id { get; set; }
    public Subject Subject { get; set; }
    public SchoolClass SchoolClass { get; set; }
    public Teacher Teacher { get; set; }
}