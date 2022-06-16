using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Services.Students;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Student")]
public class StudentController : Controller {
    private readonly IStudentsService studentsService;
    
    public StudentController(IStudentsService studentsService) {
        this.studentsService = studentsService;
    }

    /*public async Task<IActionResult> Panel() {
        Student student = await studentsService.Get
        return View();
    }*/
}