using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class SchoolSubjectViewModel {
    [Required, Display(Name = "Select subject")]
    public int SubjectId { get; set; }

    [Display(Name = "Assign classes to teacher")]
    public List<int> SelectedClassIds { get; set; } = new();
    [Required]
    public int TeacherId { get; set; }

    public string TeacherName { get; set; }

    public string TeacherSurname { get; set; }

    public IEnumerable<Subject> SubjectList { get; set; }

    public IEnumerable<SchoolClass> SchoolClassList { get; set; }
}