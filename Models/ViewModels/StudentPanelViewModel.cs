namespace SchoolRegister.Models.ViewModels; 

public class StudentPanelViewModel {
    public Student Student { get; set; }
    public IEnumerable<StudentSubject> StudentSubjects { get; set; }
}