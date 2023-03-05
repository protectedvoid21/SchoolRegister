using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.Teachers;

public class CreateTeacherViewModel {
    [Required] public string Name { get; set; }
    [Required] public string Surname { get; set; }
}