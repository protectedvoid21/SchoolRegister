using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Students;

namespace SchoolRegister.Controllers;

public class StudentController : Controller {
    private readonly IStudentsService studentsService;
    
    public StudentController(IStudentsService studentsService) {
        this.studentsService = studentsService;
    }

    public IActionResult Panel() {
        return View();
    }
}