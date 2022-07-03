using System.ComponentModel.DataAnnotations;
using SchoolRegister.Models.ViewModels.User;

namespace SchoolRegister.Models.ViewModels; 

public class MessageViewModel {
    [Required, MaxLength(50)]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string UserReceiverId { get; set; }

    public Dictionary<string, IEnumerable<UserSimpleViewModel>> UserDictionary { get; set; }
}