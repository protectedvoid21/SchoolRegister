namespace Data.Models;

public class Announcement {
    public int Id { get; set; }

    public Teacher Teacher { get; set; }

    public int? TeacherId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreateDate { get; set; }
}