using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Services.Students;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Student")]
public class StudentController : Controller {
    private readonly IStudentsService studentsService;
    private readonly UserManager<AppUser> userManager;

    public StudentController(IStudentsService studentsService, UserManager<AppUser> userManager) {
        this.studentsService = studentsService;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Panel() {
        AppUser user = await userManager.GetUserAsync(User);
        Student student = await studentsService.GetByUser(user);
        return View(student);
    }
}