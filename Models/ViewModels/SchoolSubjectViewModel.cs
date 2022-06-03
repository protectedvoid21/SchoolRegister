using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class SchoolSubjectViewModel {
    [Required]
    public Subject Subject { get; set; }
    [Required]
    [Display(Name = "Class")]
    public SchoolClass SchoolClass { get; set; }
    [Required]
    public Teacher Teacher { get; set; }

    public IEnumerable<Subject> SubjectList { get; set; }
    public IEnumerable<SchoolClass> SchoolClassList { get; set; }
    public IEnumerable<Teacher> TeacherList { get; set; }
}