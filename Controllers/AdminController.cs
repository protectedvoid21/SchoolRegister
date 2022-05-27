using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.Persons;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller {
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IPersonsService personsService;
    private readonly IGradesService gradesService;

    public AdminController(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager,
        IPersonsService personsService,
        IGradesService gradesService) {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.personsService = personsService;
        this.gradesService = gradesService;
    }

    public async Task<IActionResult> Panel() {
        var adminViewModel = new AdminPanelViewModel() {
            ClassCount = await personsService.GetClassCount(),
            StudentCount = await personsService.GetClassCount(),
            TeacherCount = await personsService.GetTeacherCount()
        };
        return View(adminViewModel);
    }

    /*public IActionResult StudentManager() {
        
    }*/
}