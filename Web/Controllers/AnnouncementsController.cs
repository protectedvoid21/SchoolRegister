using AutoMapper;
using Data.Models;
using Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Announcements;
using Services.Teachers;

namespace SchoolRegister.Controllers;

public class AnnouncementsController : Controller {
    private readonly IAnnouncementsService announcementsService;
    private readonly ITeachersService teachersService;
    private readonly UserManager<AppUser> userManager;
    private readonly IMapper mapper;

    public AnnouncementsController(IAnnouncementsService announcementsService, ITeachersService teachersService,
        UserManager<AppUser> userManager, IMapper mapper) {
        this.announcementsService = announcementsService;
        this.teachersService = teachersService;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    //todo: override authorization
    [Authorize]
    public async Task<IActionResult> Index() {
        var announcements = await announcementsService.GetAllAsync<AnnouncementViewModel>();
        announcements = announcements.OrderByDescending(a => a.CreateDate);
        return View(announcements);
    }

    [Authorize(Roles = "Teacher")]
    [HttpGet]
    public async Task<IActionResult> Add() {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);
        AnnouncementCreateViewModel announcementCreateModel = new() {
            TeacherId = teacher.Id,
        };

        return View(announcementCreateModel);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> Add(AnnouncementCreateViewModel announcementCreateModel) {
        if (!ModelState.IsValid) {
            return View(announcementCreateModel);
        }

        await announcementsService.AddAsync(announcementCreateModel.Title, announcementCreateModel.Description,
            announcementCreateModel.TeacherId);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> YourAnnouncements() {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);

        var announcements = await announcementsService.GetAllByTeacher<AnnouncementViewModel>(teacher.Id);
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

        Announcement announcement = await announcementsService.GetById(id);
        var announcementCreateModel = mapper.Map<AnnouncementCreateViewModel>(announcement);
        return View(announcementCreateModel);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> Edit(AnnouncementCreateViewModel announcementModel) {
        var user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);

        if (teacher.Id != announcementModel.TeacherId) {
            return ValidationProblem();
        }

        await announcementsService.UpdateAsync(announcementModel.Id, announcementModel.Title,
            announcementModel.Description);

        return RedirectToAction("YourAnnouncements");
    }

    [Authorize(Roles = "Teacher")]
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