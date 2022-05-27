using Microsoft.AspNetCore.Mvc;

namespace SchoolRegister.Controllers; 

public class HomeController : Controller {
    public IActionResult Index() {
        return View();
    }
}