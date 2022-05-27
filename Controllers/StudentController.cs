using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Grades;

namespace SchoolRegister.Controllers;

public class StudentController : Controller {
    private readonly IGradesService gradeService;
    
    public StudentController(IGradesService gradeService) {
        this.gradeService = gradeService;
    }

    public IActionResult Panel() {
        return View();
    }
}