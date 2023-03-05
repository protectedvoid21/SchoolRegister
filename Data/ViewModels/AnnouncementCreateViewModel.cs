using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels;

public class AnnouncementCreateViewModel {
    [Required] public int Id { get; set; }
    [Required] public int TeacherId { get; set; }
    [Required] [MaxLength(50)] public string Title { get; set; }
    [Required] public string Description { get; set; }
}