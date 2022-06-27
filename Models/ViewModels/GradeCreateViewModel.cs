using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class GradeCreateViewModel {
    public string StudentName { get; set; }
    public string StudentSurname { get; set; }

    public int StudentSubjectId { get; set; }

    public int SubjectId { get; set; }
    public string SubjectName { get; set; }

    [MaxLength(60, ErrorMessage = "Comment can not exceed 60 characters")]
    [Display(Name = "Comment (optional)")]
    public string? Comment { get; set; }
    
    [Range(0, 6), Required]
    [Display(Name = "Grade")]
    public int GradeType { get; set; }

    [Display(Name = "Addition")]
    [DefaultValue(GradeAdditionalInfo.None)]
    public GradeAdditionalInfo GradeInfo { get; set; }
}