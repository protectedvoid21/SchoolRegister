namespace SchoolRegister.Models; 

public class Message {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public AppUser? SenderUser { get; set; }
    public AppUser ReceiverUser { get; set; }
    
    public DateTime CreatedDate { get; set; }
}