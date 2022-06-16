using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Models; 

public class SchoolUser : IdentityUser {
    public ISchoolRole SchoolRole { get; set; }
}