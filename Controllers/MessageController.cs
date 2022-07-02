using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.Messages;

namespace SchoolRegister.Controllers;

[Authorize]
public class MessageController : Controller {
    private readonly UserManager<AppUser> userManager;
    private readonly IMessagesService messagesService;

    public MessageController(UserManager<AppUser> userManager, IMessagesService messagesService) {
        this.userManager = userManager;
        this.messagesService = messagesService;
    }

    public async Task<IActionResult> Index() {
        var user = await userManager.GetUserAsync(User);
        IEnumerable<Message> messages = await messagesService.GetAllReceivedMessages(user.Id);

        return View(messages);
    }

    [HttpGet]
    public IActionResult Write() {
        MessageViewModel messageModel = new();
        return View(messageModel);
    }

    /*[HttpPost]
    public async Task<IActionResult> Write() {
        var user = await userManager.GetUserAsync(User);
        

    }*/
}