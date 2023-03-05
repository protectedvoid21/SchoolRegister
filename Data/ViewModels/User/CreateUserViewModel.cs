using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.User;

public class CreateUserViewModel {
    [Required] public string UserName { get; set; }
    [Required] public string Password { get; set; }
    [Required] public int SelectedRole { get; set; }
    public List<RoleUserViewModel> RoleList { get; set; }
}

public class RoleUserViewModel {
    public string Id { get; set; }
    public string Name { get; set; }
}