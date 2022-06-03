using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class SchoolSubjectViewModel {
    [Required]
    public int SubjectId { get; set; }
    [Required]
    [Display(Name = "Class")]
    public int SchoolClassId { get; set; }
    [Required]
    public int TeacherId { get; set; }

    public IEnumerable<Subject> SubjectList { get; set; }
    public IEnumerable<SchoolClass> SchoolClassList { get; set; }
    public IEnumerable<Teacher> TeacherList { get; set; }
}