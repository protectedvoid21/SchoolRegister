namespace SchoolRegister.Models; 

public class Subject {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Student> StudentsList { get; set; }
}