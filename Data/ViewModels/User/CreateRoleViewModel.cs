using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.User;

public class CreateRoleViewModel {
    [Required] public string Name { get; set; }
}