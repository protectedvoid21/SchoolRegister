namespace Data.Models;

public class SchoolClass {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Student>? StudentsList { get; set; }
    public List<SchoolSubject>? SchoolSubjects { get; set; }
}