using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class SchoolSubjectViewModel {
    [Required]
    public int SubjectId { get; set; }
    [Required] 
    public List<ClassChoiceModel> ClassChoiceId { get; set; } = new();
    [Required]
    public int TeacherId { get; set; }

    public string? TeacherName { get; set; }
    public string? TeacherSurname { get; set; }

    public IEnumerable<Subject> SubjectList { get; set; }
    public List<SchoolClass> SchoolClassList { get; set; }
}

public class ClassChoiceModel {
    public int Id { get; set; }
    public bool IsPicked { get; set; }
}
