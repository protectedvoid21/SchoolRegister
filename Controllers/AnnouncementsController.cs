using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.Announcements;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Controllers;

[Authorize]
public class AnnouncementsController : Controller {
    private readonly IAnnouncementsService announcementsService;
    private readonly ITeachersService teachersService;
    private readonly UserManager<AppUser> userManager;

    public AnnouncementsController(IAnnouncementsService announcementsService, ITeachersService teachersService, UserManager<AppUser> userManager) {
        this.announcementsService = announcementsService;
        this.teachersService = teachersService;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index() {
        IEnumerable<Announcement> announcements = await announcementsService.GetAllAsync();
        return View(announcements);
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet]
    public async Task<IActionResult> Add() {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);
        AnnouncementCreateViewModel announcementModel = new() {
            TeacherId = teacher.Id,
        };

        return View(announcementModel);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> Add(AnnouncementCreateViewModel announcementModel) {
        if (!ModelState.IsValid) {
            return View(announcementModel);
        }

        await announcementsService.AddAsync(announcementModel.Title, announcementModel.Description, announcementModel.TeacherId);
        return RedirectToAction("Index");
    }
}