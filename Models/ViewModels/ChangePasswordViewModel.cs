using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class ChangePasswordViewModel {
    public string UserName { get; set; }
    public string UserId { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }
}