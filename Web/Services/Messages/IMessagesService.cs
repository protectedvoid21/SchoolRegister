using SchoolRegister.Models;

namespace SchoolRegister.Services.Messages; 

public interface IMessagesService {
    Task AddAsync(Message message);

    Task UpdateAsync(Message message);

    Task RemoveAsync(int messageId);

    Task<IEnumerable<Message>> GetAllReceivedMessages(string userId);

    Task<IEnumerable<Message>> GetAllSentMessages(string userId);
}