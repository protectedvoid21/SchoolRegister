using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Services.Grades;

namespace SchoolRegister.Controllers;

public class StudentController : Controller {
    private readonly ISubjectsService _subjectService;
    
    public StudentController(ISubjectsService subjectService) {
        this._subjectService = subjectService;
    }

    public IActionResult Panel() {
        return View();
    }
}