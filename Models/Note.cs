namespace SchoolRegister.Models; 

public class Note {
    public int Id { get; set; }
    public string StudentId { get; set; }
    public DateTime DateAdd { get; set; }
    public Subject Subject { get; set; }
}