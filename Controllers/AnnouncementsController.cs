using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.Announcements;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Controllers;

public class AnnouncementsController : Controller {
    private readonly IAnnouncementsService announcementsService;
    private readonly ITeachersService teachersService;
    private readonly UserManager<AppUser> userManager;

    public AnnouncementsController(IAnnouncementsService announcementsService, ITeachersService teachersService, UserManager<AppUser> userManager) {
        this.announcementsService = announcementsService;
        this.teachersService = teachersService;
        this.userManager = userManager;
    }

    //todo: override authorization
    public async Task<IActionResult> Index() {
        IEnumerable<Announcement> announcements = await announcementsService.GetAllAsync();
        announcements = announcements.OrderByDescending(a => a.CreateDate);
        return View(announcements);
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet]
    public async Task<IActionResult> Add() {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);
        AnnouncementViewModel announcementModel = new() {
            TeacherId = teacher.Id,
        };

        return View(announcementModel);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> Add(AnnouncementViewModel announcementModel) {
        if (!ModelState.IsValid) {
            return View(announcementModel);
        }

        await announcementsService.AddAsync(announcementModel.Title, announcementModel.Description, announcementModel.TeacherId);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> YourAnnouncements() {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);

        IEnumerable<Announcement> announcements = await announcementsService.GetAllByTeacher(teacher.Id);
        announcements = announcements.OrderByDescending(a => a.CreateDate);
        return View(announcements);
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);

        if (await announcementsService.IsOwner(id, teacher.Id) == false) {
            return ValidationProblem();
        }

        var announcement = await announcementsService.GetById(id);
        AnnouncementViewModel announcementModel = new() {
            Id = id,
            Title = announcement.Title,
            Description = announcement.Description,
            TeacherId = (int)announcement.TeacherId //todo: nullability could be done better
        };
        return View(announcementModel);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> Edit(AnnouncementViewModel announcementModel) {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);

        if (teacher.Id != announcementModel.TeacherId) {
            return ValidationProblem();
        }

        Announcement announcement = await announcementsService.GetById(announcementModel.Id);
        announcement.Title = announcementModel.Title;
        announcement.Description = announcementModel.Description;

        await announcementsService.UpdateAsync(announcement);

        return RedirectToAction("YourAnnouncements");
    }

    public async Task<IActionResult> Delete(int id) {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);

        var announcement = await announcementsService.GetById(id);

        if (announcement.TeacherId != teacher.Id) {
            return ValidationProblem();
        }

        await announcementsService.RemoveAsync(id);
        return RedirectToAction("YourAnnouncements");
    }
}