using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels.User;

public class CreateRoleViewModel {
    [Required]
    public string Name { get; set; }
}