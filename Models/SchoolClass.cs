namespace SchoolRegister.Models; 

public class SchoolClass {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Student> Students { get; set; }
}