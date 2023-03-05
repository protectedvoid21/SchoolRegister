namespace Data.Models;

public class SchoolSubject {
    public int Id { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public SchoolClass SchoolClass { get; set; }
    public int SchoolClassId { get; set; }

    public Teacher Teacher { get; set; }
    public int TeacherId { get; set; }

    public List<StudentSubject> StudentSubjects { get; set; }
}