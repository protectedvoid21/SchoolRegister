﻿using Data.Models;
using Data.ViewModels;
using Data.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Messages;
using Services.Students;
using Services.Teachers;

namespace SchoolRegister.Controllers;

[Authorize]
public class MessageController : Controller {
    private readonly UserManager<AppUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly ITeachersService teachersService;
    private readonly IStudentsService studentsService;
    private readonly IMessagesService messagesService;

    public MessageController(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITeachersService teachersService,
        IStudentsService studentsService,
        IMessagesService messagesService) {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.teachersService = teachersService;
        this.studentsService = studentsService;
        this.messagesService = messagesService;
    }

    public async Task<IActionResult> Index() {
        var user = await userManager.GetUserAsync(User);
        var messages = (await messagesService.GetAllReceivedMessages(user.Id)).OrderByDescending(m => m.CreatedDate);

        return View(messages);
    }

    public async Task<IActionResult> View(int id) {
        var user = await userManager.GetUserAsync(User);
        var message = await messagesService.GetById(id);

        if (message.SenderUser != user && message.ReceiverUser != user) {
            return BadRequest();
        }

        return View(message);
    }

    [HttpGet]
    public async Task<IActionResult> Write() {
        Dictionary<string, IEnumerable<UserSimpleViewModel>> userGrouped = new();

        if (!User.IsInRole("Student")) {
            var studentList = await studentsService.GetAllAsync();
            List<UserSimpleViewModel> studentModelList = new();
            foreach (var student in studentList) {
                studentModelList.Add(new UserSimpleViewModel {
                    Id = student.User.Id,
                    Name = student.User.Name,
                    Surname = student.User.Surname,
                });
            }

            userGrouped.Add("Students", studentModelList);
        }

        var teacherList = await teachersService.GetAllAsync();
        List<UserSimpleViewModel> teacherModelList = new();
        foreach (var teacher in teacherList) {
            teacherModelList.Add(new UserSimpleViewModel {
                Id = teacher.User.Id,
                Name = teacher.User.Name,
                Surname = teacher.User.Surname,
            });
        }

        userGrouped.Add("Teachers", teacherModelList);

        MessageViewModel messageModel = new() {
            UserDictionary = userGrouped
        };
        return View(messageModel);
    }

    [HttpPost]
    public async Task<IActionResult> Write(MessageViewModel messageModel) {
        if (!ModelState.IsValid) {
            return View(messageModel);
        }

        var user = await userManager.GetUserAsync(User);
        var receiverUser = await userManager.FindByIdAsync(messageModel.UserReceiverId);

        if (user == receiverUser) {
            ModelState.AddModelError("", "You can not send message to yourself");
            return View(messageModel);
        }

        if (await userManager.IsInRoleAsync(user, "Student")) {
            if (await userManager.IsInRoleAsync(receiverUser, "Student")) {
                return Forbid();
            }
        }

        await messagesService.AddAsync(messageModel.Title, messageModel.Description, user.Id,
            messageModel.UserReceiverId);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SentList() {
        var user = await userManager.GetUserAsync(User);
        var messages = await messagesService.GetAllSentMessages(user.Id);

        return View(messages);
    }

    public async Task<IActionResult> HiddenList() {
        var user = await userManager.GetUserAsync(User);
        var hiddenMessages = (await messagesService.GetAllReceivedMessages(user.Id)).Where(m => m.IsVisible == false);

        return View(hiddenMessages);
    }

    public async Task<IActionResult> Revoke(int id) {
        var user = await userManager.GetUserAsync(User);

        if (!await messagesService.IsReceiver(id, user.Id)) {
            return Forbid();
        }

        await messagesService.ChangeVisibility(id, true);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id) {
        var user = await userManager.GetUserAsync(User);

        if (!await messagesService.IsReceiver(id, user.Id)) {
            return Forbid();
        }

        await messagesService.ChangeVisibility(id, false);
        return RedirectToAction("Index");
    }
}