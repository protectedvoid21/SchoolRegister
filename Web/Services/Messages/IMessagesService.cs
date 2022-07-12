using SchoolRegister.Models;

namespace SchoolRegister.Services.Messages; 

public interface IMessagesService {
    Task AddAsync(string title, string description, string senderUserId, string receiverUserId);

    Task UpdateAsync(int id, string title, string description);

    Task RemoveAsync(int id);

    Task<Message> GetById(int id);

    Task<bool> IsReceiver(int messageId, string userId);

    Task ChangeVisibility(int id, bool visibility);

    Task<IEnumerable<Message>> GetAllReceivedMessages(string userId);

    Task<IEnumerable<Message>> GetAllSentMessages(string userId);
}