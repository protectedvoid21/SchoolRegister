namespace SchoolRegister.Models.ViewModels.Grades;

public class GradeViewModel {
    public int Id { get; set; }
    public string SubjectName { get; set; }
    public string GradeName { get; set; }
    public string? Comment { get; set; }
    public DateTime DateAdd { get; set; }
}