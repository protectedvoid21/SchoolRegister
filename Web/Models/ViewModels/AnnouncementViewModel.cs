using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class AnnouncementViewModel {
    public int Id { get; set; }
    public int TeacherId { get; set; }
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
}