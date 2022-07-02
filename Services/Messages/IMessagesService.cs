using SchoolRegister.Models;

namespace SchoolRegister.Services.Messages; 

public interface IMessagesService {
    Task AddAsync();

    Task<IEnumerable<Message>> GetAllReceivedMessages(string userId);
}