using SchoolRegister.Models;

namespace SchoolRegister.Services.Messages; 

public class MessagesService : IMessagesService {
    private readonly SchoolRegisterContext schoolContext;

    public MessagesService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task AddAsync(Message message) {
        await schoolContext.AddAsync(message);
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Message message) {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(int messageId) {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Message>> GetAllReceivedMessages(string userId) {
        return schoolContext.Messages.Where(m => m.ReceiverUser.Id == userId);
    }

    public async Task<IEnumerable<Message>> GetAllSentMessages(string userId) {
        return schoolContext.Messages.Where(m => m.SenderUser.Id == userId);
    }
}