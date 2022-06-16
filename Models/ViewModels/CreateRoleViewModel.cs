using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class CreateRoleViewModel {
    [Required]
    public string Name { get; set; }
}