using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels;

public class SchoolClassViewModel {
    [Required] [Display(Name = "Class name")] public string Name { get; set; }
}