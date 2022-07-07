namespace SchoolRegister.Models.ViewModels.Students;

public class StudentPanelViewModel {
    public Student Student { get; set; }
    public IEnumerable<StudentSubject> StudentSubjects { get; set; }
}