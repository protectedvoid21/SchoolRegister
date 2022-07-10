using SchoolRegister.Models;

namespace SchoolRegister.Services.Messages; 

public class MessagesService : IMessagesService {
    private readonly SchoolRegisterContext schoolContext;

    public MessagesService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
    }
    
    public async Task AddAsync(string title, string description, string senderUserId, string receiverUserId) {
        Message message = new() {
            Title = title,
            Description = description,
            CreatedDate = DateTime.Now,
            IsVisible = true,
            SenderUserId = senderUserId,
            ReceiverUserId = receiverUserId
        };
        await schoolContext.AddAsync(message);
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, string title, string description) {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(int id) {
        throw new NotImplementedException();
    }

    public async Task<bool> IsReceiver(int id, string userId) {
        var message = await schoolContext.Messages.FindAsync(id);
        return message.ReceiverUser.Id == userId;
    }

    public async Task DeleteForReceiver(int id) {
        var message = await schoolContext.Messages.FindAsync(id);
        message.IsVisible = false;
    }

    public async Task<IEnumerable<Message>> GetAllReceivedMessages(string userId) {
        return schoolContext.Messages.Where(m => m.ReceiverUserId == userId);
    }

    public async Task<IEnumerable<Message>> GetAllSentMessages(string userId) {
        return schoolContext.Messages.Where(m => m.SenderUserId == userId);
    }
}