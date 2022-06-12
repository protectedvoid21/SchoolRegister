using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class CreateTeacherViewModel {
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
}