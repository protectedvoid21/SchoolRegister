using Microsoft.AspNetCore.Mvc;

namespace SchoolRegister.Controllers; 

public class HomeController : Controller {
    public IActionResult Index() {
        if (User.IsInRole("Student")) {
            return RedirectToAction("Panel", "Student");
        }

        return View("Index");
    }
}