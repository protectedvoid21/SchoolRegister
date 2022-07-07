using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class SubjectViewModel {
    [Required]
    [Display(Name = "Subject name")]
    public string Name { get; set; }
}