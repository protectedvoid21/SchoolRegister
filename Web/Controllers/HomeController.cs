using Microsoft.AspNetCore.Mvc;

namespace SchoolRegister.Controllers;

public class HomeController : Controller {
    public IActionResult Index() {
        if (User.IsInRole("Student")) {
            return RedirectToAction("Index", "Student");
        }

        if (User.IsInRole("Teacher")) {
            return RedirectToAction("Index", "Teacher");
        }

        if (User.IsInRole("Admin")) {
            return RedirectToAction("Panel", "Admin");
        }

        return View("Index");
    }
}