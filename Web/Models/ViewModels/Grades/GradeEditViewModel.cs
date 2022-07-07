using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels.Grades;

public class GradeEditViewModel {
    public int Id { get; set; }

    public string StudentName { get; set; }
    public string StudentSurname { get; set; }
    public string SubjectName { get; set; }

    [Range(GlobalConstants.MinGrade, GlobalConstants.MaxGrade)]
    public int GradeType { get; set; }
    public GradeAdditionalInfo GradeInfo { get; set; }

    public string? Comment { get; set; }
}