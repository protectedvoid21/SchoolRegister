using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Services.Messages;
using Xunit;

namespace SchoolRegister.Tests; 

public class MessagesServiceTests {
    private readonly MessagesService messagesService;
    
    public MessagesServiceTests() {
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContext = new SchoolContext(options);
        messagesService = new MessagesService(dbContext);
    }

    [Theory]
    [InlineData("Title1", "Description1")]
    [InlineData("Title2", "Description2")]
    [InlineData("BądTitlż", "BądDęśćription")]
    public async Task Add_NewMessage_MessageIsVisibleInDatabase(string title, string description) {
        const string senderId = "senderId";
        const string receiverId = "receiverId";
        
        await messagesService.AddAsync(title, description, senderId, receiverId);

        Assert.Single(await messagesService.GetAllReceivedMessages(receiverId));
        Assert.Single(await messagesService.GetAllSentMessages(senderId));
    }

    [Fact]
    public async Task Toggle_Visibility_ChangesVisibilityOfMessageForReceiver() {
        await messagesService.AddAsync("Title", "Description", senderUserId: "1", receiverUserId: "2");

        var messageList = await messagesService.GetAllReceivedMessages("2");
        Message message = messageList.First();
        
        Assert.True(message.IsVisible);

        await messagesService.ChangeVisibility(message.Id, false);
        
        Assert.False(message.IsVisible);
    } 
}