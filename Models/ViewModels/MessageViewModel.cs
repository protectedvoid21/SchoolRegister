using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class MessageViewModel {
    [Required, MaxLength(50)]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }

    public int UserReceiverId { get; set; }
}