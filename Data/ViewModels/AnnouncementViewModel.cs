namespace Data.ViewModels;

public class AnnouncementViewModel {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string TeacherName { get; set; }

    public string TeacherSurname { get; set; }

    public DateTime CreateDate { get; set; }
}