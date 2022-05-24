using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;

namespace SchoolRegister.Controllers; 

public class HomeController : Controller {
    public IActionResult Index() {
        return View();
    }
}