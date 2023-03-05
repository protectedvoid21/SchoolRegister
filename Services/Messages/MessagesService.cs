using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Messages;

public class MessagesService : IMessagesService {
    private readonly SchoolContext schoolContext;

    public MessagesService(SchoolContext schoolContext) {
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

    public async Task<Message> GetById(int id) {
        var message = await schoolContext.Messages
            .Include(m => m.SenderUser)
            .Include(m => m.ReceiverUser)
            .FirstAsync(m => m.Id == id);
        return message;
    }

    public async Task<bool> IsReceiver(int id, string userId) {
        var message = await schoolContext.Messages.FindAsync(id);
        return message.ReceiverUser.Id == userId;
    }

    public async Task ChangeVisibility(int id, bool visibility) {
        var message = await schoolContext.Messages.FindAsync(id);
        message.IsVisible = visibility;

        schoolContext.Update(message);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Message>> GetAllReceivedMessages(string userId) {
        return schoolContext.Messages
            .Include(m => m.SenderUser)
            .Where(m => m.ReceiverUserId == userId);
    }

    public async Task<IEnumerable<Message>> GetAllSentMessages(string userId) {
        return schoolContext.Messages
            .Include(m => m.ReceiverUser)
            .Where(m => m.SenderUserId == userId);
    }
}