using Microsoft.AspNetCore.Mvc;

namespace SchoolRegister.Controllers; 

public class StudentController : Controller {
    public IActionResult Panel() {
        return View();
    }
}