using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Models;

public class AppUser : IdentityUser<int> {
    public string Name { get; set; }
    public string Surname { get; set; }
}