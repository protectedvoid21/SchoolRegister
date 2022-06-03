using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class SchoolClassViewModel {
    [Required]
    [Display(Name = "Class name")]
    public string Name { get; set; }
}