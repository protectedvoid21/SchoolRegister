using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SchoolRegister.Models; 

public class IdentityAppContext : IdentityDbContext {
    public IdentityAppContext(DbContextOptions<IdentityAppContext> options) : base(options) {}
}