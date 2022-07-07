using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels.Students;

public class CreateStudentViewModel {
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public int SchoolClassId { get; set; }

    public string SchoolClassName { get; set; }
}