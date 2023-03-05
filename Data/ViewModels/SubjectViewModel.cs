using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels;

public class SubjectViewModel {
    [Required] [Display(Name = "Subject name")] public string Name { get; set; }
}