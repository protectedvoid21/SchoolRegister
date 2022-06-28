using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class GradeEditViewModel {
    public int Id { get; set; }

    public string StudentName { get; set; }
    public string StudentSurname { get; set; }
    public string SubjectName { get; set; }

    [Range(0, 6)]
    public int GradeType { get; set; }
    public GradeAdditionalInfo GradeInfo { get; set; }

    public string? Comment { get; set; }
}