namespace SchoolRegister.Models.ViewModels.Teacher;

public class TeacherStudentSubjectViewModel {
    public string ClassName { get; set; }
    public string SubjectName { get; set; }
    public IEnumerable<StudentSubject> StudentSubjects { get; set; }
}