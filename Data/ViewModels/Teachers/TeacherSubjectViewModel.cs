using Data.Models;

namespace Data.ViewModels.Teachers;

public class TeacherSubjectViewModel {
    public SchoolClass SchoolClass { get; set; }
    public List<Subject> SubjectList { get; set; }
}