using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class AnnouncementCreateViewModel {
    public int TeacherId { get; set; }
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
}