using SchoolRegister.Models;

namespace SchoolRegister.Services.Messages; 

public interface IMessagesService {
    Task AddAsync(string title, string description, string senderUserId, string receiverUserId);

    Task UpdateAsync(int id, string title, string description);

    Task RemoveAsync(int id);

    Task<bool> IsReceiver(int messasgeId, string userId);

    Task DeleteForReceiver(int id);

    Task<IEnumerable<Message>> GetAllReceivedMessages(string userId);

    Task<IEnumerable<Message>> GetAllSentMessages(string userId);
}